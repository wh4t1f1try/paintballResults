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
    private IDuplicatesChecker checker;
    private IDataRecordValidator dataRecordValidator;
    private ICsvDataStringValidator dataStringValidator;
    private ImportService importService;
    private IStringToGameResultMapper mapper;
    private IGameResultRepository repository;
    private IStreamToStringConverter streamToStringConverter;
    private IStringToDataRecordConverter stringToDataRecordConverter;
    private IGameResultValidator validator;

    [TestInitialize]
    public void Setup()
    {
        this.mapper = Substitute.For<IStringToGameResultMapper>();
        this.validator = Substitute.For<IGameResultValidator>();
        this.streamToStringConverter = Substitute.For<IStreamToStringConverter>();
        this.repository = Substitute.For<IGameResultRepository>();
        this.dataStringValidator = Substitute.For<ICsvDataStringValidator>();
        this.checker = Substitute.For<IDuplicatesChecker>();
        this.stringToDataRecordConverter = Substitute.For<IStringToDataRecordConverter>();
        this.dataRecordValidator = Substitute.For<IDataRecordValidator>();
        this.importService = new ImportService(this.mapper, this.validator, this.streamToStringConverter,
            this.repository, this.dataStringValidator, this.checker, this.stringToDataRecordConverter,
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
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };
        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.dataStringValidator.Received().Validate(dataStrings);
    }

    [TestMethod]
    public void ImportGameResult_Call_IStringToDataRecordConverter_Convert()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };
        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.stringToDataRecordConverter.Received().Convert(dataStrings);
    }

    [TestMethod]
    public void ImportGameResult_Call_IDataRecordValidator_Validate()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        IList<string[]> dataRecord = new List<string[]>
        {
            new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4" }
        };

        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        this.stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecord);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.dataRecordValidator.Received().Validate(dataRecord);
    }

    [TestMethod]
    public void ImportGameResult_Call_IStringToGameResultMapper_MapGameResult()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        IList<string[]> dataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Virst Factory Lódz", "Ballistics Göttingen", "4", "5" }
        };

        IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecord.Skip(1));

        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        this.stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecord);
        this.dataRecordValidator.Validate(dataRecord);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.mapper.Received()
            .MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(dataRecordsWithoutHeader)));
    }

    [TestMethod]
    public void ImportGameResult_Call_DuplicatesChecker_CheckDuplicates()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        IList<string[]> dataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Virst Factory Lódz", "Ballistics Göttingen", "4", "5" }
        };

        IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecord.Skip(1));

        IList<GameResult> gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        this.stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecord);
        this.dataRecordValidator.Validate(dataRecord);
        this.mapper.MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(dataRecordsWithoutHeader)))
            .Returns(gameResults);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.checker.Received().CheckDuplicates(gameResults);
    }

    [TestMethod]
    public void ImportGameResult_Call_GameResultValidator_Validate()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        IList<string[]> dataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Virst Factory Lódz", "Ballistics Göttingen", "4", "5" }
        };

        IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecord.Skip(1));

        IList<GameResult> gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        this.stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecord);
        this.dataRecordValidator.Validate(dataRecord);
        this.mapper.MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(dataRecordsWithoutHeader)))
            .Returns(gameResults);
        this.checker.CheckDuplicates(gameResults);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.validator.Received().Validate(gameResults);
    }

    [TestMethod]
    public void ImportGameResult_Call_GameResultRepository_InsertAllGameResults()
    {
        //Arrange
        Stream stream = CreateFile().OpenReadStream();
        IList<string> dataStrings = new List<string>
        {
            "Spiel;Tag;Team 1;Team 2;T1 MP;T2 MP",
            "1;1;Virst Factory Lódz;Ballistics Göttingen;4;5"
        };

        IList<string[]> dataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Virst Factory Lódz", "Ballistics Göttingen", "4", "5" }
        };

        IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecord.Skip(1));

        IList<GameResult> gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        this.streamToStringConverter.Convert(stream).Returns(dataStrings);
        this.dataStringValidator.Validate(dataStrings);
        this.stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecord);
        this.dataRecordValidator.Validate(dataRecord);
        this.mapper.MapGameResult(Arg.Is<List<string[]>>(x => x.SequenceEqual(dataRecordsWithoutHeader)))
            .Returns(gameResults);
        this.checker.CheckDuplicates(gameResults);
        this.validator.Validate(gameResults);
        //Act
        this.importService.ImportGameResults(stream);
        //Assert
        this.repository.Received().InsertAllGameResults(gameResults);
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