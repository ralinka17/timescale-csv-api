using Microsoft.EntityFrameworkCore;
using TimescaleApi.Data;
using TimescaleApi.Models;

namespace TimescaleApi.Services;

public class ValueService : IValueService
{
    private readonly AppDbContext _context;

    public ValueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Value>> GetLast10ByFileNameAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("fileName is required");

        return await _context.Values
            .AsNoTracking()
            .Where(v => v.FileName == fileName)
            .OrderByDescending(v => v.Date)
            .Take(10)
            .ToListAsync();
    }
}