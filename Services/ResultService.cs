using Microsoft.EntityFrameworkCore;
using TimescaleApi.Data;
using TimescaleApi.Dtos;
using TimescaleApi.Models;

namespace TimescaleApi.Services;

public class ResultService : IResultService
{
    private readonly AppDbContext _context;

    public ResultService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Result>> GetFilteredResultsAsync(ResultFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.FileName))
        {
            filter.FileName = Path.GetFileNameWithoutExtension(filter.FileName);
        }

        var query = _context.Results
            .AsNoTracking() // для чтения — ускоряет и экономит память
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FileName))
        {
            query = query.Where(r => r.FileName == filter.FileName);
        }

        if (filter.MinStartDate.HasValue)
        {
            query = query.Where(r => r.MinDate >= filter.MinStartDate.Value);
        }

        if (filter.MaxStartDate.HasValue)
        {
            query = query.Where(r => r.MinDate <= filter.MaxStartDate.Value);
        }

        if (filter.MinAvgValue.HasValue)
        {
            query = query.Where(r => r.AvgValue >= filter.MinAvgValue.Value);
        }

        if (filter.MaxAvgValue.HasValue)
        {
            query = query.Where(r => r.AvgValue <= filter.MaxAvgValue.Value);
        }

        if (filter.MinAvgExecutionTime.HasValue)
        {
            query = query.Where(r => r.AvgExecutionTime >= filter.MinAvgExecutionTime.Value);
        }

        if (filter.MaxAvgExecutionTime.HasValue)
        {
            query = query.Where(r => r.AvgExecutionTime <= filter.MaxAvgExecutionTime.Value);
        }

        // Можно добавить сортировку, например по MinDate DESC
        query = query.OrderByDescending(r => r.MinDate);

        return await query.ToListAsync();
    }
}