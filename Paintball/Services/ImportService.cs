namespace Paintball.Services
{
    using Paintball.Abstractions.Converters;
    using Paintball.Abstractions.Mappers;
    using Paintball.Abstractions.Services;
    using Paintball.Abstractions.Validators;
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;

    public class ImportService : IImportService
    {
        private readonly IDuplicatesChecker checker;
        private readonly IDataRecordValidator dataRecordValidator;
        private readonly ICsvDataStringValidator dataStringValidator;
        private readonly IStringToGameResultMapper mapper;
        private readonly IGameResultRepository repository;
        private readonly IStreamToStringConverter streamToStringConverter;
        private readonly IStringToDataRecordConverter stringToDataRecordConverter;
        private readonly IGameResultValidator validator;


        public ImportService
        (
            IStringToGameResultMapper mapper, IGameResultValidator validator,
            IStreamToStringConverter streamToStringConverter, IGameResultRepository repository,
            ICsvDataStringValidator dataStringValidator, IDuplicatesChecker checker,
            IStringToDataRecordConverter stringToDataRecordConverter, IDataRecordValidator dataRecordValidator
        )
        {
            this.dataStringValidator = dataStringValidator;
            this.checker = checker;
            this.mapper = mapper;
            this.validator = validator;
            this.streamToStringConverter = streamToStringConverter;
            this.repository = repository;
            this.checker = checker;
            this.stringToDataRecordConverter = stringToDataRecordConverter;
            this.dataRecordValidator = dataRecordValidator;
        }

        public void ImportGameResults(Stream? stream)
        {
            IList<string> dataStrings = this.streamToStringConverter.Convert(stream);
            this.dataStringValidator.Validate(dataStrings);

            IList<string[]> dataRecords = this.stringToDataRecordConverter.Convert(dataStrings);
            this.dataRecordValidator.Validate(dataRecords);

            IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecords.Skip(1));

            IList<GameResult> gameResults = this.mapper.MapGameResult(dataRecordsWithoutHeader);

            this.checker.CheckDuplicates(gameResults);
            this.validator.Validate(gameResults);
            this.repository.InsertAllGameResults(gameResults);
        }
    }
}