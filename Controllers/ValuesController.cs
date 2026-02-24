using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimescaleApi.Services;

namespace TimescaleApi.Controllers;

[ApiController]
[Route("api/values")]
public class ValuesController : ControllerBase
{
    private readonly ICsvService _csvService;
    private readonly IValueService _valueService;

    public ValuesController(ICsvService csvService, IValueService valueService)
    {
        _csvService = csvService;
        _valueService = valueService;
    }

    /// <summary>
    /// Upload and process CSV file.
    /// </summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            await _csvService.ProcessCsvAsync(file);
            return Ok("File processed successfully.");
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Controller catch: " + ex.ToString());
            return StatusCode(500, $"Internal error: {ex.Message}\nDetails: {ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получить последние 10 значений по имени файла (по убыванию даты)
    /// </summary>
    /// <param name="fileName">Имя файла без расширения</param>
    [HttpGet("last10")]

    public async Task<IActionResult> GetLast10([FromQuery] string fileName)
    {
        fileName = Path.GetFileNameWithoutExtension(fileName ?? "");

        try
        {
            var values = await _valueService.GetLast10ByFileNameAsync(fileName);
            return Ok(values);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetLast10 error: " + ex.ToString());
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
}