namespace Sporty.DataModel
{
    public class RuleViolation
    {
        public RuleViolation(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public RuleViolation(string errorMessage, string propertyName)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }

        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }
    }
}