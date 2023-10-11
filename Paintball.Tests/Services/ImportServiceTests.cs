#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Paintball.Tests.Services;

using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using Paintball.Abstractions.Converters;
using Paintball.Abstractions.Mappers;
using Paintball.Abstractions.Validators;
using Paintball.Database.Abstractions.Entities;
using Paintball.Database.Abstractions.Repositories;
using Paintball.Services;

[TestClass]
public class ImportServiceTests
{
    private IList<string[]> dataRecords;
    private IList<string[]> dataRecordsWithoutHeader;
    private IDataRecordValidator dataRecordValidator;
    private IList<string> dataStrings;
    private ICsvDataStringValidator dataStringValidator;
    private IDuplicatesChecker duplicatesChecker;
    private IGameResultRepository gameResultRepository;
    private IList<GameResult> gameResults;
    private IGameResultValidator gameResultValidator;
    private ImportService importService;
    private Stream stream;
    private IStreamToStringConverter streamToStringConverter;
    private IStringToDataRecordConverter stringToDataRecordConverter;
    private IStringToGameResultMapper stringToGameResultMapper;

    [TestInitialize]
    public void Setup()
    {
        this.stream = CreateFile().OpenReadStream();
        this.dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        this.dataRecords = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Virst Factory Lódz", "Ballistics Göttingen", "4", "5" }
        };

        this.dataRecordsWithoutHeader = new List<string[]>(this.dataRecords.Skip(1));

        this.gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        this.stringToGameResultMapper = Substitute.For<IStringToGameResultMapper>();
        this.gameResultValidator = Substitute.For<IGameResultValidator>();
        this.streamToStringConverter = Substitute.For<IStreamToStringConverter>();
        this.gameResultRepository = Substitute.For<IGameResultRepository>();
        this.dataStringValidator = Substitute.For<ICsvDataStringValidator>();
        this.duplicatesChecker = Substitute.For<IDuplicatesChecker>();
        this.stringToDataRecordConverter = Substitute.For<IStringToDataRecordConverter>();
        this.dataRecordValidator = Substitute.For<IDataRecordValidator>();
        this.importService = new ImportService(this.stringToGameResultMapper, this.gameResultValidator,
            this.streamToStringConverter,
            this.gameResultRepository, this.dataStringValidator, this.duplicatesChecker,
            this.stringToDataRecordConverter,
            this.dataRecordValidator);
    }

    [TestMethod]
    public void ImportGameResults_Call_IStreamToStringConverter_Convert()
    {
        //Arrange
        MemoryStream stream = new();
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.streamToStringConverter.Received().Convert(stream);
    }

    [TestMethod]
    public void ImportGameReuslts_Call_ICsvDataStringValidator_Validate()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.dataStringValidator.Received().Validate(this.dataStrings);
    }

    [TestMethod]
    public void ImportGameResult_Call_IStringToDataRecordConverter_Convert()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.stringToDataRecordConverter.Received().Convert(this.dataStrings);
    }

    [TestMethod]
    public void ImportGameResult_Call_IDataRecordValidator_Validate()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        this.stringToDataRecordConverter.Convert(this.dataStrings).Returns(this.dataRecords);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.dataRecordValidator.Received().Validate(this.dataRecords);
    }

    [TestMethod]
    public void ImportGameResult_Call_IStringToGameResultMapper_MapGameResult()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        this.stringToDataRecordConverter.Convert(this.dataStrings).Returns(this.dataRecords);
        this.dataRecordValidator.Validate(this.dataRecords);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.stringToGameResultMapper.Received()
            .MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(this.dataRecordsWithoutHeader)));
    }

    [TestMethod]
    public void ImportGameResult_Call_DuplicatesChecker_CheckDuplicates()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        this.stringToDataRecordConverter.Convert(this.dataStrings).Returns(this.dataRecords);
        this.dataRecordValidator.Validate(this.dataRecords);
        this.stringToGameResultMapper
            .MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(this.dataRecordsWithoutHeader)))
            .Returns(this.gameResults);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.duplicatesChecker.Received().CheckDuplicates(this.gameResults);
    }

    [TestMethod]
    public void ImportGameResult_Call_GameResultValidator_Validate()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        this.stringToDataRecordConverter.Convert(this.dataStrings).Returns(this.dataRecords);
        this.dataRecordValidator.Validate(this.dataRecords);
        this.stringToGameResultMapper
            .MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(this.dataRecordsWithoutHeader)))
            .Returns(this.gameResults);
        this.duplicatesChecker.CheckDuplicates(this.gameResults);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.gameResultValidator.Received().Validate(this.gameResults);
    }

    [TestMethod]
    public void ImportGameResult_Call_GameResultRepository_InsertAllGameResults()
    {
        //Arrange
        this.streamToStringConverter.Convert(this.stream).Returns(this.dataStrings);
        this.dataStringValidator.Validate(this.dataStrings);
        this.stringToDataRecordConverter.Convert(this.dataStrings).Returns(this.dataRecords);
        this.dataRecordValidator.Validate(this.dataRecords);
        this.stringToGameResultMapper
            .MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(this.dataRecordsWithoutHeader)))
            .Returns(this.gameResults);
        this.duplicatesChecker.CheckDuplicates(this.gameResults);
        this.gameResultValidator.Validate(this.gameResults);
        //Act
        this.importService.ImportGameResults(this.stream);
        //Assert
        this.gameResultRepository.Received().InsertAllGameResults(this.gameResults);
    }

    private static IFormFile CreateFile()
    {
        StringBuilder content = new StringBuilder();
        content.AppendLine("Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP")
            .AppendLine("1;1;Virst Factory Lódz;Ballistics Göttingen;4;5");

        string fileName = "test.pdf";
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        return file;
    }
}