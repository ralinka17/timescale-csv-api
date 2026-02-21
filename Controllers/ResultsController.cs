using Microsoft.AspNetCore.Mvc;
using TimescaleApi.Dtos;
using TimescaleApi.Services;

namespace TimescaleApi.Controllers;

[ApiController]
[Route("api/results")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        _resultService = resultService;
    }

    /// <summary>
    /// Получить список результатов с фильтрами
    /// </summary>
    /// <param name="filter">Параметры фильтрации</param>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ResultFilterDto filter)
    {
        var results = await _resultService.GetFilteredResultsAsync(filter);
        return Ok(results);
    }
}