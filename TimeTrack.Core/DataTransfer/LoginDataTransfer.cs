using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer
{
    public class LoginDataTransfer
    {
        [MaxLength(350)]
        public string Mail { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
    }
}