using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(ChangePasswordDataTransfer))]
    public class ChangePasswordDataTransfer
    {
        [Required, MaxLength(30)]
        public string Password { get; set; }
    }
}