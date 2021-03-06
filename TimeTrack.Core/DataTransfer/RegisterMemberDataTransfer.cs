using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.DataTransfer
{
    public class RegisterMemberDataTransfer
    {
        [MaxLength(25)]
        public string GivenName { get; set; }
        [MaxLength(25)]
        public string Surname { get; set; }
        [Required, MaxLength(350)]
        public string Mail { get; set; }
        [Required, MaxLength(30)]
        public string Password { get; set; }
    }
}