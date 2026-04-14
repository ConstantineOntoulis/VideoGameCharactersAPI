using VideoGameCharactersAPI.Services;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Unit tests for the small pure rules used by query pagination logic
public class QueryRulesTests
{
    [Fact]
    public void NormalizePage_WhenPageIsLessThanOne_ReturnsOne()
    {
        //Arrange:Use an invalid page value smaller than 1
        var page = 0;
        //Act:Run the normalization rule
        var result = QueryRules.NormalizePage(page);
        //Assert:The rule should normalize invalid values to page 1
        Assert.Equal(1, result);
    }

    [Fact]
    public void NormalizePageSize_WhenPageSizeIsLessThanOne_ReturnsDefaultTen()
    {
        //Arrange:Use an invalid page size smaller than 1
        var pageSize = 0;
        // Act:Run the normalization rule
        var result = QueryRules.NormalizePageSize(pageSize);
        //Assert:The rule should normalize invalid values to the default size of 10
        Assert.Equal(10, result);
    }

    [Fact]
    public void NormalizePageSize_WhenPageSizeIsGreaterThanFifty_ReturnsFifty()
    {
        //Arrange:Use an overly large page size value
        var pageSize = 999;

        //Act:Run the normalization rule
        var result = QueryRules.NormalizePageSize(pageSize);

        //Assert:The rule should cap large values at 50
        Assert.Equal(50, result);
    }
}