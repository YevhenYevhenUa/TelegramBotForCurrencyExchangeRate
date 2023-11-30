using Microsoft.Extensions.Logging;
using Moq;
using Task11.TelegramBot.Service;
using Task11.TelegramBot.Service_Interface;

namespace Task11.TelegramBot.Tests;

public class DataValidationServiceTests
{
    public IDataValidatorsService _sut;
    public Mock<ILogger<DataValidatorService>> _mockLogger;
    public Mock<IDateTimeProvider> _mockTimeProvider;

    public DataValidationServiceTests()
    {
        _mockLogger = new Mock<ILogger<DataValidatorService>>();
        _mockTimeProvider = new Mock<IDateTimeProvider>();
        _sut = new DataValidatorService(_mockLogger.Object, _mockTimeProvider.Object);
    }

    [Theory]
    [InlineData(null, "Wrong message type, try again! Use pattern 'USD dd.mm.yyyy' for correct response")]
    [InlineData("USD", "")]
    public void DataValidatorService_CheckUserInputMessage_ShouldReturnEmtyStringOrError(string currency, string actualResult)
    {
        //Arrange

        //Act
        var expectResult = _sut.CheckUserInputMessage(currency);
        //Assert
        Assert.Equal(expectResult, actualResult);
    }

    [Fact]
    public void DataValidationService_IsTheDateLessOrEqualToNow_ReturnTrueIfLess()
    {
        //Arrange
        DateTime testDate = new DateTime(2023, 11, 26);
        _mockTimeProvider.Setup(t=>t.TimeNow).Returns(testDate);
        DateTime inputDate = new DateTime(2020, 11, 20);
        //Act
        var expectResult = _sut.IsTheDateLessOrEqualToNow(inputDate);
        //Assert
        Assert.True(expectResult);
    }

    [Fact]
    public void DataValidationService_IsTheDateLessOrEqualToNow_ReturnTrueIfEqual()
    {
        //Arrange
        DateTime testDate = new DateTime(2023, 11, 26);
        _mockTimeProvider.Setup(t => t.TimeNow).Returns(testDate);
        DateTime inputDate = new DateTime(2023, 11, 26);
        //Act
        var expectResult = _sut.IsTheDateLessOrEqualToNow(inputDate);
        //Assert
        Assert.True(expectResult);
    }

    [Fact]
    public void DataValidationService_IsTheDateLessOrEqualToNow_ReturnFalseIfBigger()
    {
        //Arrange
        DateTime testDate = new DateTime(2023, 11, 26);
        _mockTimeProvider.Setup(t => t.TimeNow).Returns(testDate);
        DateTime inputDate = new DateTime(2025, 11, 20);
        //Act
        var expectResult = _sut.IsTheDateLessOrEqualToNow(inputDate);
        //Assert
        Assert.False(expectResult);
    }


}