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
        private readonly IDuplicatesChecker _checker;
        private readonly IDataRecordValidator _dataRecordValidator;
        private readonly ICsvDataStringValidator _dataStringValidator;
        private readonly IStringToGameResultMapper _mapper;
        private readonly IGameResultRepository _repository;
        private readonly IStreamToStringConverter _streamToStringConverter;
        private readonly IStringToDataRecordConverter _stringToDataRecordConverter;
        private readonly IGameResultValidator _validator;


        public ImportService
        (
            IStringToGameResultMapper mapper, IGameResultValidator validator,
            IStreamToStringConverter streamToStringConverter, IGameResultRepository repository,
            ICsvDataStringValidator dataStringValidator, IDuplicatesChecker checker,
            IStringToDataRecordConverter stringToDataRecordConverter, IDataRecordValidator dataRecordValidator
        )
        {
            this._dataStringValidator = dataStringValidator;
            this._checker = checker;
            this._mapper = mapper;
            this._validator = validator;
            this._streamToStringConverter = streamToStringConverter;
            this._repository = repository;
            this._checker = checker;
            this._stringToDataRecordConverter = stringToDataRecordConverter;
            this._dataRecordValidator = dataRecordValidator;
        }

        public void ImportGameResults(Stream? stream)
        {
            IList<string> dataStrings = this._streamToStringConverter.Convert(stream);
            this._dataStringValidator.Validate(dataStrings);

            IList<string[]> dataRecords = this._stringToDataRecordConverter.Convert(dataStrings);
            this._dataRecordValidator.Validate(dataRecords);

            IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecords.Skip(1));

            IList<GameResult> gameResults = this._mapper.MapGameResult(dataRecordsWithoutHeader);

            this._checker.CheckDuplicates(gameResults);
            this._validator.Validate(gameResults);
            this._repository.InsertAllGameResults(gameResults);
        }
    }
}