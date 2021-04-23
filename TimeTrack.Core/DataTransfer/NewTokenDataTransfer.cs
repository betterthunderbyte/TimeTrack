using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer
{
    public class NewTokenDataTransfer
    {
        [MaxLength(2000)]
        public string Token { get; set; }
    }
}