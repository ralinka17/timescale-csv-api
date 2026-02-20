using Microsoft.AspNetCore.Http;
using TimescaleApi.Models;

namespace TimescaleApi.Services;

public interface ICsvService
{
    Task ProcessCsvAsync(IFormFile file);
}