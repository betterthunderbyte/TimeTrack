namespace TimeTrack.Core
{
    public class ValidationResult
    {
        public bool Successful { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        
        public static implicit operator bool(ValidationResult validationResult)
        {
            return validationResult.Successful;
        }
    }
}