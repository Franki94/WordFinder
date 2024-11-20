using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WordFinder.Service.ConfigValues;

namespace WordFinders.Service.Tests;

[TestFixture]
public class WordFinderTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void IsValidMatrix_WhenMatrixHasMoreThanAllowedSize_ShouldReturnFalse()
    {
        //Arrange
        var options = Options.Create<WordFinderConfig>(new WordFinderConfig(){MaxSize = 3});
        var matrix = new List<string>()
        {
            "abcd",
            "abcd",
            "abcd",
            "abcd"
        };
        //Action
        var sut = new WordFinder.Service.WordFinder(matrix,options);
        var result = sut.IsValidMatrix();
        
        //Assert
        result.Should().BeFalse();
    }
    
    [Test]
    [TestCase("abc", "abcd","abc")]
    [TestCase("abcd", "abcd","abc")]
    [TestCase("", "","")]
    public void IsValidMatrix_WhenMatrixDoesNotHaveSameHorizontalThanVertical_ShouldReturnFalse(string firstGroup, string secondGroup, string thirdGroup)
    {
        //Arrange
        var options = Options.Create<WordFinderConfig>(new WordFinderConfig(){MaxSize = 64});
        var matrix = new List<string>()
        {
            firstGroup,
            secondGroup,
            thirdGroup
        };
        //Action
        var sut = new WordFinder.Service.WordFinder(matrix,options);
        var result = sut.IsValidMatrix();
        
        //Assert
        result.Should().BeFalse();
    }
    
    [Test]
    [TestCase("abc", "abc","abc")]
    public void IsValidMatrix_WhenMatrixDoesNotHaveSameHorizontalThanVertical_ShouldReturnTrue(string firstGroup, string secondGroup, string thirdGroup)
    {
        //Arrange
        var options = Options.Create<WordFinderConfig>(new WordFinderConfig(){MaxSize = 64});
        var matrix = new List<string>()
        {
            firstGroup,
            secondGroup,
            thirdGroup
        };
        //Action
        var sut = new WordFinder.Service.WordFinder(matrix,options);
        var result = sut.IsValidMatrix();
        
        //Assert
        result.Should().BeTrue();
    }
    
    [Test]
    public void Find_WhenValidMatrixUniqueWordsToFind_ShouldReturnFoundOccurrencesSorted()
    {
        //Arrange
        var options = Options.Create<WordFinderConfig>(new WordFinderConfig(){MaxSize = 64});
        var matrix = new List<string>()
        {
            "abc",
            "afc",
            "aaf"
        };
        var words = new List<string>()
        {
            "aa","cc","fa"
        };
        
        //Action
        var sut = new WordFinder.Service.WordFinder(matrix,options);
        var result = sut.Find(words);
        
        //Assert
        result.Should().BeEquivalentTo(new List<string>
        {
            "aa","cc","fa"
        });
    }
    
    [Test]
    public void Find_WhenValidMatrixDuplicateWordsToFind_ShouldReturnFoundOccurrencesSorted()
    {
        //Arrange
        var options = Options.Create<WordFinderConfig>(new WordFinderConfig(){MaxSize = 64});
        var matrix = new List<string>()
        {
            "abc",
            "afc",
            "aaf"
        };
        var words = new List<string>()
        {
            "aa","cc","fa","aa","cc"
        };
        
        //Action
        var sut = new WordFinder.Service.WordFinder(matrix,options);
        var result = sut.Find(words);
        
        //Assert
        result.Should().BeEquivalentTo(new List<string>
        {
            "aa","cc","fa"
        });
    }
    
}