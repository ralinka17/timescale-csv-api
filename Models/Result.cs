using System.ComponentModel.DataAnnotations;

public class Result
{
    [Key]
    [MaxLength(255)]
    public string FileName { get; set; } = null!;

    public double TimeDeltaSeconds { get; set; }
    public DateTime MinDate { get; set; }
    public double AvgExecutionTime { get; set; }
    public double AvgValue { get; set; }
    public double MedianValue { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }

    // Можно добавить CreatedAt, UpdatedAt и т.д.
}