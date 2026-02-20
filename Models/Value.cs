using System.ComponentModel.DataAnnotations;

namespace TimescaleApi.Models;

public class Value

{

public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public DateTime Date { get; set; }
    public double ValueIndicator { get; set; }
    [Required]
    [Range(0, double.MaxValue)]
    public double ExecutionTime { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double MetricValue { get; set; }   // ← было Value → стало MetricValue

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = null!;
}