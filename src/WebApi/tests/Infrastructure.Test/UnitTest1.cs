using Xunit;
using FluentAssertions;

namespace API.Starter.Infrastructure.Test;

public class UnitTest1
{
    [Fact]
    public void Test1_ShouldPass()
    {
        // Arrange
        var expected = true;

        // Act  
        var actual = true;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Test2_ShouldWork()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.Where(x => x > 2);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainInOrder(3, 4, 5);
    }
}
