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
    private IDuplicatesChecker _checker;
    private IDataRecordValidator _dataRecordValidator;
    private ICsvDataStringValidator _dataStringValidator;
    private ImportService _importService;
    private IStringToGameResultMapper _mapper;
    private IGameResultRepository _repository;
    private IStreamToStringConverter _streamToStringConverter;
    private IStringToDataRecordConverter _stringToDataRecordConverter;
    private IGameResultValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        this._mapper = Substitute.For<IStringToGameResultMapper>();
        this._validator = Substitute.For<IGameResultValidator>();
        this._streamToStringConverter = Substitute.For<IStreamToStringConverter>();
        this._repository = Substitute.For<IGameResultRepository>();
        this._dataStringValidator = Substitute.For<ICsvDataStringValidator>();
        this._checker = Substitute.For<IDuplicatesChecker>();
        this._stringToDataRecordConverter = Substitute.For<IStringToDataRecordConverter>();
        this._dataRecordValidator = Substitute.For<IDataRecordValidator>();
        this._importService = new ImportService(this._mapper, this._validator, this._streamToStringConverter,
            this._repository, this._dataStringValidator, this._checker, this._stringToDataRecordConverter,
            this._dataRecordValidator);
    }

    [TestMethod]
    public void ImportGameResults_ShouldCallAllServices_WhenStreamIsNotNull()
    {
        // Arrange
        MemoryStream stream = new MemoryStream();

        IList<string> dataStrings = new List<string>
        {
            "Spiel,Tag, Team 1, Team 2, T1 MP, T2 MP",
            "1,1, Team 1, Team 2,0,5"
        };

        IList<string[]> dataRecords = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Team 1", "Team 2", "0", "5" }
        };

        IList<GameResult> gameResults = new List<GameResult>
        {
            new GameResult
            {
                Id = 1,
                GameDay = 1,
                TeamOne = "Team 1",
                TeamTwo = "Team 2",
                TeamOneMatchPoints = 0,
                TeamTwoMatchPoints = 5
            },
        };

        this._streamToStringConverter.Convert(stream).Returns(dataStrings);
        this._stringToDataRecordConverter.Convert(dataStrings).Returns(dataRecords);
        IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecords.Skip(1));
        this._mapper.MapGameResult(dataRecordsWithoutHeader).Returns(gameResults);

        // Act
        this._importService.ImportGameResults(stream);

        // Assert
        this._streamToStringConverter.Received().Convert(stream);
        this._dataStringValidator.Received().Validate(dataStrings);
        this._stringToDataRecordConverter.Received().Convert(dataStrings);
        this._dataRecordValidator.Received().Validate(dataRecords);

        //this._mapper.Received().MapGameResult(dataRecordsWithoutHeader);
        //this._checker.Received().CheckDuplicates(gameResults);
        //this._validator.Received().Validate(gameResults);
        //this._repository.Received().InsertAllGameResults(gameResults);
    }
}