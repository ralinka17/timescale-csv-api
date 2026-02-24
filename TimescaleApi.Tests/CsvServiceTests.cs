using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using TimescaleApi.Data;
using TimescaleApi.Models;
using TimescaleApi.Services;
using Xunit;

namespace TimescaleApi.Tests;

public class CsvServiceTests
{
    [Fact]
    public async Task ProcessCsvAsync_ValidSingleRow_SavesData()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var service = new CsvService(context);

        var csvContent = "2025-01-01T12:00:00.0000Z;1.5;10.0";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.csv");
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.Length).Returns(stream.Length);

        // Act
        await service.ProcessCsvAsync(fileMock.Object);

        // Assert
        var values = await context.Values.ToListAsync();
        values.Should().HaveCount(1);
        values[0].FileName.Should().Be("test");
        values[0].ExecutionTime.Should().Be(1.5);
        values[0].ValueIndicator.Should().Be(10.0);

        var result = await context.Results.FirstOrDefaultAsync();
        result.Should().NotBeNull();
        result.AvgValue.Should().Be(10.0);
    }
}