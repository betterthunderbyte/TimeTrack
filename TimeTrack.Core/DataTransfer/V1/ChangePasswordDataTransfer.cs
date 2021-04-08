using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer.V1
{
    public class ChangePasswordDataTransfer
    {
        [Required, MaxLength(30)]
        public string Password { get; set; }
    }
}