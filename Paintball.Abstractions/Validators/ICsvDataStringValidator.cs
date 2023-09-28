namespace Paintball.Abstractions.Validators
{
    public interface ICsvDataStringValidator
    {
        public void Validate(IList<string> dataStrings);
    }
}