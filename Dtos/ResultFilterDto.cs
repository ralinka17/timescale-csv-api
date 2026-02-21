namespace TimescaleApi.Dtos;

public class ResultFilterDto
{
    public string? FileName { get; set; }

    public DateTime? MinStartDate { get; set; }
    public DateTime? MaxStartDate { get; set; }

    public double? MinAvgValue { get; set; }
    public double? MaxAvgValue { get; set; }

    public double? MinAvgExecutionTime { get; set; }
    public double? MaxAvgExecutionTime { get; set; }
}