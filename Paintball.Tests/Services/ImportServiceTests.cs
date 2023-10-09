#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Paintball.Tests.Services;

using FluentAssertions;
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
        this.dataStringValidator = Substitute.For<ICsvDataStringValidator>();
        this.checker = Substitute.For<IDuplicatesChecker>();
        this.mapper = Substitute.For<IStringToGameResultMapper>();
        this.validator = Substitute.For<IGameResultValidator>();
        this.streamToStringConverter = Substitute.For<IStreamToStringConverter>();
        this.repository = Substitute.For<IGameResultRepository>();
        this.stringToDataRecordConverter = Substitute.For<IStringToDataRecordConverter>();
        this.dataRecordValidator = Substitute.For<IDataRecordValidator>();
        this.repository = Substitute.For<IGameResultRepository>();

        this.importService = new ImportService(this.mapper, this.validator, this.streamToStringConverter,
            this.repository, this.dataStringValidator, this.checker, this.stringToDataRecordConverter,
            this.dataRecordValidator
        );
    }


    [TestMethod]
    public void ImportGameResults_Invokes_DuplicatesChecker_CheckDuplicates()
    {
        MemoryStream stream = new MemoryStream();
        IList<GameResult> gameResults = new List<GameResult>
        {
            new()
            {
                Id = 1,
                Gameday = 1,
                TeamOne = "Team One",
                TeamTwo = "Team Two",
                TeamOneMatchPoints = 1,
                TeamTwoMatchPoints = 1
            }
        };

        this.importService.ImportGameResults(stream);

        this.checker.Invoking(c => c.CheckDuplicates(gameResults));
    }

    [TestMethod]
    public void ImportGameResults_Invokes_Mapper_MapGameResults()
    {
        MemoryStream stream = new MemoryStream();

        IList<string[]> dataRecords = new List<string[]>
        {
            new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "0", "1" }
        };

        this.importService.ImportGameResults(stream);
    }
}