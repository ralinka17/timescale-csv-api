using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TimescaleApi.Dtos;
using TimescaleApi.Services;

namespace TimescaleApi.Controllers;

[ApiController]
[Route("api/results")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        ArgumentNullException.ThrowIfNull(resultService);
        _resultService = resultService;
    }

    /// <summary>
    /// Получить список результатов с фильтрами
    /// </summary>
    /// <param name="filter">Параметры фильтрации (все необязательные)</param>
    /// <returns>Список отфильтрованных результатов</returns>
    /// <response code="200">Успешно возвращены результаты</response>
    /// <response code="400">Некорректные параметры запроса</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Получить результаты с фильтрами",
        Description = "Возвращает список записей из таблицы Results. Все параметры фильтрации необязательные."
    )]
    [SwaggerResponse(200, "Список результатов", typeof(List<Result>))]
    public async Task<IActionResult> Get([FromQuery] ResultFilterDto filter)
    {
        var results = await _resultService.GetFilteredResultsAsync(filter);
        return Ok(results);
    }
}