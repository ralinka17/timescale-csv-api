using Xunit;
using FluentAssertions;

namespace TimescaleApi.Tests;

public class SmokeTests
{
    [Fact]
    public void BasicMathTest()
    {
        (1 + 1).Should().Be(2);
    }
}