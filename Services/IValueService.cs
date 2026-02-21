using TimescaleApi.Models;

namespace TimescaleApi.Services;

public interface IValueService
{
    Task<List<Value>> GetLast10ByFileNameAsync(string fileName);
}