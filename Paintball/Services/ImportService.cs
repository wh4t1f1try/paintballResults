using Paintball.Abstractions.Converters;
using Paintball.Abstractions.Mappers;
using Paintball.Abstractions.Services;
using Paintball.Abstractions.Validators;
using Paintball.Database.Abstractions.Repositories;

namespace Paintball.Services
{
    public class ImportService : IImportService
    {
        private readonly ICsvDataStringValidator _dataStringValidator;
        private readonly IDuplicatesChecker _checker;
        private readonly IStringToGameResultMapper _mapper;
        private readonly IGameResultValidator _validator;
        private readonly IStreamToStringConverter _streamToStringConverter;
        private readonly IGameResultRepository _repository;
        private readonly IStringToDataRecordConverter _stringToDataRecordConverter;
        private readonly IDataRecordValidator _dataRecordValidator;


        public ImportService
        (
            IStringToGameResultMapper mapper,
            IGameResultValidator validator,
            IStreamToStringConverter streamToStringConverter,
            IGameResultRepository repository,
            ICsvDataStringValidator dataStringValidator,
            IDuplicatesChecker checker,
            IStringToDataRecordConverter stringToDataRecordConverter,
            IDataRecordValidator dataRecordValidator
        )
        {
            _dataStringValidator = dataStringValidator;
            _checker = checker;
            _mapper = mapper;
            _validator = validator;
            _streamToStringConverter = streamToStringConverter;
            _repository = repository;
            _checker = checker;
            _stringToDataRecordConverter = stringToDataRecordConverter;
            _dataRecordValidator = dataRecordValidator;
        }

        public void ImportGameResults(Stream? stream)
        {
            var dataStrings = _streamToStringConverter.Convert(stream);
            _dataStringValidator.Validate(dataStrings);

            var dataRecords = _stringToDataRecordConverter.Convert(dataStrings);
            _dataRecordValidator.Validate(dataRecords);

            IList<string[]> dataRecordsWithoutHeader = new List<string[]>(dataRecords.Skip(1));

            var gameResults = _mapper.MapGameResult(dataRecordsWithoutHeader);

            _checker.CheckDuplicates(gameResults);
            _validator.Validate(gameResults);
            _repository.InsertAllGameResults(gameResults);
            _repository.Save();
        }
    }
}