using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class NewTokenDataTransfer
    {
        [MaxLength(2000)]
        public string Token { get; set; }
    }
}