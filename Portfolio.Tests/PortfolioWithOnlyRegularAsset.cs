using NUnit.Framework;
using static Portfolio.Tests.helpers.AssetsFileLinesBuilder;
using static Portfolio.Tests.helpers.TestingPortfolioBuilder;

namespace Portfolio.Tests;

public class PortfolioWithOnlyRegularAsset
{
    [TestCase(50)]
    [TestCase(1)] // off point for asset value boundary between (-inf, 0] y (0, +inf] when asset date before now
    public void when_value_is_more_than_zero_it_decreases_by_2_before_now(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate("2024/01/15").WithValue(assetValue))
            .OnDate("2025/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue - 2}"));
    }
    
    [TestCase(0)] // on point for asset value boundary between (-inf, 0] y (0, +inf] when asset date before now
    [TestCase(-10)]
    public void when_value_is_equal_or_less_than_zero_it_remains_the_same_before_now(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate("2024/01/15").WithValue(assetValue))
            .OnDate("2025/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue}"));
    }
    
    [Test] // 1 day before now: off point for asset date in days boundary between (-inf, 0) y [0, +inf] 
    public void value_decreases_by_2_one_day_before_right_now()
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate("2023/01/01").WithValue(6))
            .OnDate("2023/01/02")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("4"));
    }
    
    [Test] // 0 days: on point for asset date in days boundary between (-inf, 0) y [0, +inf] 
    public void value_decreases_by_1_right_now()
    {
        const string now = "2023/01/01";
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate(now).WithValue(6))
            .OnDate(now)
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("5"));
    }
    
    [TestCase(0)] // on point for asset value boundary between (-inf, 0] y (0, +inf] when asset date after now
    [TestCase(-10)]
    public void when_value_is_equal_or_less_than_zero_it_remains_the_same_after_now(int  assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate("2024/01/15").WithValue(assetValue))
            .OnDate("2023/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue}"));
    }

    [TestCase(100)]
    [TestCase(1)] // off point for asset value boundary between (-inf, 0] y (0, +inf] when asset date after now
    public void value_decreases_by_1_after_now(int  assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Some Regular Asset").FromDate("2024/01/15").WithValue(assetValue))
            .OnDate("2023/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue - 1}"));
    }
}