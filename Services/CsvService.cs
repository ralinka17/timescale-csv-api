using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimescaleApi.Data;
using TimescaleApi.Dtos;
using TimescaleApi.Models;

namespace TimescaleApi.Services;

public class CsvService : ICsvService
{
    private readonly AppDbContext _context;

    public CsvService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ProcessCsvAsync(IFormFile file)
    {
        if (file == null || file.Length == 0 || !file.FileName.EndsWith(".csv"))
        {
            throw new BadHttpRequestException("Invalid file: must be non-empty CSV.");
        }

        var fileName = Path.GetFileNameWithoutExtension(file.FileName);  // Без .csv

        // Чтение CSV потоково
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",  // Разделитель ;
            HasHeaderRecord = false,  // Нет заголовков
            MissingFieldFound = null  // Игнор missing, но валидируем позже
        };

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, config);

        var records = new List<CsvRecord>();
        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<CsvRecord>();
            if (record != null)
            {
                records.Add(record);
            }
        }

        // Валидация количества строк
        if (records.Count < 1 || records.Count > 10000)
        {
            throw new BadHttpRequestException($"Invalid row count: {records.Count} (must be 1-10000).");
        }

        // Парсинг и валидация каждой строки + преобразование в Value
        var values = new List<Value>();
        var parsedDates = new List<DateTime>();
        var parsedExecutionTimes = new List<double>();
        var parsedValues = new List<double>();

        var minDate = DateTime.MaxValue;
        var maxDate = DateTime.MinValue;

        foreach (var record in records)
        {
            // Валидация: отсутствие значений
            if (string.IsNullOrWhiteSpace(record.DateString) ||
                string.IsNullOrWhiteSpace(record.ExecutionTimeString) ||
                string.IsNullOrWhiteSpace(record.ValueIndicatorString))
            {
                throw new BadHttpRequestException("Missing fields in row.");
            }

            // Парсинг Date: формат ГГГГ-ММ-ДДTчч:мм:сс.ммммZ (ISO с Z)
            if (!DateTime.TryParseExact(record.DateString, "yyyy-MM-ddTHH:mm:ss.ffffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
            {
                throw new BadHttpRequestException($"Invalid Date format: {record.DateString}");
            }

            // Валидация Date
            var now = DateTime.UtcNow;
            var minAllowedDate = new DateTime(2000, 1, 1);
            if (date > now || date < minAllowedDate)
            {
                throw new BadHttpRequestException($"Date out of range: {date} (must be 2000-01-01 to now).");
            }

            // Парсинг ExecutionTime
            if (!double.TryParse(record.ExecutionTimeString, NumberStyles.Float, CultureInfo.InvariantCulture, out var execTime) || execTime <= 0)
            {
                throw new BadHttpRequestException($"Invalid ExecutionTime: {record.ExecutionTimeString} (must be > 0).");
            }

            // Парсинг ValueIndicator
            if (!double.TryParse(record.ValueIndicatorString, NumberStyles.Float, CultureInfo.InvariantCulture, out var valueInd) || valueInd <= 0)
            {
                throw new BadHttpRequestException($"Invalid Value: {record.ValueIndicatorString} (must be > 0).");
            }

            // Добавляем
            values.Add(new Value
            {
                Date = date,
                ExecutionTime = execTime,
                ValueIndicator = valueInd,
                FileName = fileName
            });

            parsedDates.Add(date);
            parsedExecutionTimes.Add(execTime);
            parsedValues.Add(valueInd);

            if (date < minDate) minDate = date;
            if (date > maxDate) maxDate = date;
        }

        // Расчёты аггрегатов
        var timeDeltaSeconds = (maxDate - minDate).TotalSeconds;
        var avgExecTime = parsedExecutionTimes.Average();
        var avgValue = parsedValues.Average();
        var minValue = parsedValues.Min();
        var maxValue = parsedValues.Max();

        // Медиана: сортируем и берём середину
        parsedValues.Sort();
        var medianValue = parsedValues.Count % 2 == 0
            ? (parsedValues[parsedValues.Count / 2 - 1] + parsedValues[parsedValues.Count / 2]) / 2
            : parsedValues[parsedValues.Count / 2];

        var result = new Result
        {
            FileName = fileName,
            TimeDeltaSeconds = timeDeltaSeconds,
            MinDate = minDate,
            AvgExecutionTime = avgExecTime,
            AvgValue = avgValue,
            MedianValue = medianValue,
            MaxValue = maxValue,
            MinValue = minValue
        };

        // Транзакция: удаление старых + добавление новых
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Удаляем старые Values и Result по FileName
            await _context.Values.Where(v => v.FileName == fileName).ExecuteDeleteAsync();
            await _context.Results.Where(r => r.FileName == fileName).ExecuteDeleteAsync();

            // Добавляем новые (batch для эффективности)
            _context.Values.AddRange(values);
            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;  // Пробрасываем ошибку вверх
        }
    }
}