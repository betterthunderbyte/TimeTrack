using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TimeTrack.Core.DataTransfer
{
    [XmlRoot(nameof(NewTokenDataTransfer))]
    public class NewTokenDataTransfer
    {
        [MaxLength(2000)]
        public string Token { get; set; }
    }
}