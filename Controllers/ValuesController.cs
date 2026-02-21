using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimescaleApi.Services;

namespace TimescaleApi.Controllers;

[ApiController]
[Route("api/values")]
public class ValuesController : ControllerBase
{
    private readonly ICsvService _csvService;

    public ValuesController(ICsvService csvService)
    {
        _csvService = csvService;
    }

    /// <summary>
    /// Upload and process CSV file.
    /// </summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]  // ← опционально, но полезно для ясности в Swagger
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
            Console.WriteLine("Controller catch: " + ex.ToString()); // или используй ILogger
            return StatusCode(500, $"Internal error: {ex.Message}\nDetails: {ex.InnerException?.Message}");
        }
    }
}