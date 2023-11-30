using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Task11.TelegramBot.DTO;
using Task11.TelegramBot.Service;
using Task11.TelegramBot.Service_Interface;

namespace Task11.TelegramBot.Tests;
public class DataFormattingServiceTests
{
    private readonly IDataFormattingService _sut;
    private readonly Mock<ILogger<DataFormattingService>> _mockLogger;
    private readonly IOptions<AppSettings> _appSettings;
    private readonly Mock<IDateTimeProvider> _mockTimeProvider;
    public DataFormattingServiceTests()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configFilePath = Path.Combine(basePath, "Appsettings.json");
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(configFilePath, optional: false)
        .Build();
        _appSettings = Options.Create(configuration.GetSection("telegrambot").Get<AppSettings>());
        _mockLogger = new Mock<ILogger<DataFormattingService>>();
        _mockTimeProvider = new Mock<IDateTimeProvider>();
        _sut = new DataFormattingService(_mockLogger.Object, _appSettings, _mockTimeProvider.Object);
    }

    [Fact]
    public void DataFormattingService_GetValidatedDate_ShouldReturnEmptyStringOrError()
    {
        //Arrange
        var inputDate = new DateTime(2021, 12, 20);
        //var expectDate = new DateTime(2021, 12, 20);
        DateCurrencyRecord dateCurrencyTest = new DateCurrencyRecord("USD", inputDate);
        //Act
        var expectResult = _sut.GetValidatedDate(dateCurrencyTest);
        //Assert
        Assert.Equal(inputDate, expectResult);
    }
    
    [Fact]
    public void DataFormattingService_GetMessageRecord_ShouldReturnRecordWithFullData()
    {
        //Arrange
        string testData = "USD 20.12.2021";
        string sepCurrnecy = "USD";
        DateTime testDate = new DateTime(2021, 12, 20);
        DateCurrencyRecord testRecord = new DateCurrencyRecord(sepCurrnecy, testDate);
        //Act
        var expectResult = _sut.GetMessageRecord(testData);

        //Assert
        Assert.Equal(testRecord.Date, expectResult.Date);
        Assert.Equal(testRecord.Currency, expectResult.Currency);
    }

    [Fact]
    public void DataFormattingService_GetMessageRecord_ShouldReturnRecordWitHalfFilledData()
    {
        //Arrange
        string testData = "USD";
        string sepCurrnecy = "USD";
        DateCurrencyRecord testRecord = new DateCurrencyRecord(sepCurrnecy, null);
        //Act
        var expectResult = _sut.GetMessageRecord(testData);

        //Assert
        Assert.Equal(testRecord.Date, expectResult.Date);
        Assert.Null(expectResult.Date);
        Assert.Equal(testRecord.Currency, expectResult.Currency);
    }

    [Fact]
    public void DataFormattingService_GetMessageRecord_ShouldReturnRecordWitEmptyData()
    {
        //Arrange
        string testData = "";
        string sepCurrnecy = string.Empty;
        DateCurrencyRecord testRecord = new DateCurrencyRecord(sepCurrnecy, null);
        //Act
        var expectResult = _sut.GetMessageRecord(testData);

        //Assert
        Assert.Equal(testRecord.Date, expectResult.Date);
        Assert.Null(expectResult.Date);
        Assert.Equal(testRecord.Currency, expectResult.Currency);
    }


    [Fact]
    public void DataFormattingService_GetMessageRecord_ShouldReturnRecordWitDefaultDate()
    {
        //Arrange
        string testData = "USD 20?12?2021";
        string sepCurrnecy = "USD";
        DateTime testDate = default(DateTime);
        DateCurrencyRecord testRecord = new DateCurrencyRecord(sepCurrnecy, testDate);
        //Act
        var expectResult = _sut.GetMessageRecord(testData);

        //Assert
        Assert.Equal(testRecord.Date, expectResult.Date);
        Assert.Equal(testRecord.Currency, expectResult.Currency);
    }

}
