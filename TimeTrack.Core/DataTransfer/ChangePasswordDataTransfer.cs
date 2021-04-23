using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer
{
    public class ChangePasswordDataTransfer
    {
        [Required, MaxLength(30)]
        public string Password { get; set; }
    }
}