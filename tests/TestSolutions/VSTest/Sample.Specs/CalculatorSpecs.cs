using Xunit;

namespace Sample.Specs;

public class CalculatorSpecs
{
    [Fact]
    public void Adds_two_numbers()
    {
        // Act
        int sum = 1 + 2;

        // Assert
        Assert.Equal(3, sum);
    }

    [Fact(Skip = "Shows that skipped tests are counted.")]
    public void Is_skipped()
    {
    }
}
