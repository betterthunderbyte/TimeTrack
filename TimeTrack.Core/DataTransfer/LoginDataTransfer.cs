using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(LoginDataTransfer))]
    public class LoginDataTransfer
    {
        private string _mail;
        
        [MaxLength(350)]
        public string Mail
        {
            get { return _mail; }
            set
            {
                _mail = value.Trim();
            } 
        }
  
        [MaxLength(30)]
        public string Password { get; set; }

        public ValidationResult IsValid()
        {
            if (string.IsNullOrWhiteSpace(Mail))
            {
                return new ValidationResult()
                {
                    Message = "E-Mail nicht vorhanden!",
                    Successful = false
                };
            }
            
            if (string.IsNullOrWhiteSpace(Password))
            {
                return new ValidationResult()
                {
                    Message = "Passwort nicht vorhanden!",
                    Successful = false
                };
            }

            if (!Mail.Contains("@"))
            {
                return new ValidationResult()
                {
                    Message = "In der E-Mail fehlt ein '@'-Zeichen!",
                    Successful = false
                };
            }
            
            if (!Mail.Contains("."))
            {
                return new ValidationResult()
                {
                    Message = "In der E-Mail fehlt ein '.'-Zeichen!",
                    Successful = false
                };
            }

            return new ValidationResult()
            {
                Successful = true
            };
        }
        
    }
}