using TimescaleApi.Dtos;
using TimescaleApi.Models;

namespace TimescaleApi.Services;

public interface IResultService
{
    Task<List<Result>> GetFilteredResultsAsync(ResultFilterDto filter);
}