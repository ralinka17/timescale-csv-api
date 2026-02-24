using System.Globalization;
using CsvHelper.Configuration.Attributes;

namespace TimescaleApi.Dtos;

public class CsvRecord
{
    [Name("Date")]  
    public string? DateString { get; set; }

    [Name("ExecutionTime")]
    public string? ExecutionTimeString { get; set; }

    [Name("Value")]
    public string? ValueIndicatorString { get; set; }
}