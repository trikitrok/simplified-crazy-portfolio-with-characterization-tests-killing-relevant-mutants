using NUnit.Framework;
using static Portfolio.Tests.helpers.AssetsFileLinesBuilder;
using static Portfolio.Tests.helpers.TestingPortfolioBuilder;

namespace Portfolio.Tests;

public class PortfolioWithOnlyLotteryPrediction
{
    [Test]
    public void value_drops_to_zero_before_now()
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/15").WithValue(50))
            .OnDate("2025/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("0"));
    }

    [TestCase("2024/04/15")]
    [TestCase("2024/01/12")] // 11 days, on point for days boundary between [6, 11) y [11, +inf]
    public void value_grows_by_1_11_days_or_more_after_now(string assetDate)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate(assetDate).WithValue(50))
            .OnDate("2024/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("51"));
    }

    [TestCase("2024/01/11")] // 10 days, off point for days boundary between [6, 11) and [11, +inf]
    [TestCase("2024/01/07")] // 6 days, on point for days boundary between [0, 6) and [6, 11)
    public void value_grows_by_2_less_than_11_days_after_now(string assetDate)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate(assetDate).WithValue(50))
            .OnDate("2024/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("52"));
    }

    [TestCase("2024/01/06")] // 5 days, off point for days boundary between [0, 6) and [6, 11)
    public void value_grows_by_3_less_than_6_days_after_now(string assetDate)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate(assetDate).WithValue(50))
            .OnDate("2024/01/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo("53"));
    }

    
    // For more than 11 days after now
    [TestCase(800)] // on point for value of asset boundary between (-inf, 800) and [800, +inf] 
    [TestCase(801)]
    public void more_than_11_days_after_now_when_value_is_equal_or_more_than_800_it_remains_the_same(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/12").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue}"));
    }
    
    [TestCase(-794)]
    [TestCase(799)] // off point for value of asset boundary between (-inf, 800) and [800, +inf] 
    public void more_than_11_days_after_now_when_value_is_less_than_800_it_grows_by_1(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/12").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 1}"));
    }
    //----------------------
    
    // For 11 > days >= 6 after now
    [TestCase(-794)] 
    [TestCase(798)] // off point for value of asset boundary between (-inf, 799) and [799, 800)
    public void less_than_11_days_and_more_than_6_after_now_when_value_is_less_than_799_it_grows_by_2(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/08").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 2}"));
    }
    
    [TestCase(799)] // on point for value of asset boundary between (-inf, 799) and [799, 800)
                    // off point for value of asset boundary between [799, 800) and [800, +inf)
    public void less_than_11_days_and_more_than_6_after_now_when_value_is_equal_to_799_it_grows_by_1(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/08").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 1}"));
    }
    
    [TestCase(800)] // on point for value of asset boundary between [799, 800) and [800, +inf)
    [TestCase(900)]  
    public void less_than_11_days_and_more_than_6_after_now_when_value_is_equal_or_more_than_800_it_remains_the_same(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/08").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue}"));
    }
    //----------------------
    
    
    // For days < 6 after now
    [TestCase(-774)]
    [TestCase(797)] // off point for value of asset boundary between (-inf, 798) and [798, 799)
    public void less_than_6_days_after_now_when_value_is_less_than_798_it_grows_by_3(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/06").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 3}"));
    }
    
    [TestCase(798)] // on point for value of asset boundary between (-inf, 798) and [798, 799)
                    // off point for value of asset boundary between [798, 799) and [799, 800)
    public void less_than_6_days_after_now_when_value_is_equal_to_798_it_grows_by_2(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/06").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 2}"));
    }
    
                    
    [TestCase(799)] // on point for value of asset boundary between [798, 799) and [799, 800)
                    // off point for value of asset boundary between [799, 800) and [800, +inf)
    public void less_than_6_days_after_now_when_value_is_equal_to_799_it_grows_by_1(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/06").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();
    
        portfolio.ComputePortfolioValue();
    
        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue + 1}"));
    }
    
    [TestCase(800)] // on point for value of asset boundary between [799, 800) and [800, +inf)
    [TestCase(1000)]
    public void less_than_6_days_after_now_when_value_is_equal_or_more_than_800_it_remains_the_same(int assetValue)
    {
        var portfolio = APortFolio()
            .With(AnAsset().DescribedAs("Lottery Prediction").FromDate("2024/04/06").WithValue(assetValue))
            .OnDate("2024/04/01")
            .Build();

        portfolio.ComputePortfolioValue();

        Assert.That(portfolio._messages[0], Is.EqualTo($"{assetValue}"));
    }
}