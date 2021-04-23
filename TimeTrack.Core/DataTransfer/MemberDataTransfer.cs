using System;
using System.ComponentModel.DataAnnotations;
using TimeTrack.Core.Model;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core.DataTransfer
{
    public class MemberDataTransfer : IUseCaseConverter<MemberEntity>
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }

        public bool Active { get; set; }
        [MaxLength(25)]
        public string GivenName { get; set; }
        [MaxLength(25)]
        public string Surname { get; set; }
        [Required, MaxLength(350)]
        public string Mail { get; set; }

        public bool MailConfirmed { get; set; }
        
        public bool RenewPassword { get; set; }
        
        public MemberEntity.MemberRole Role { get; set; }
        
        public virtual void To(out MemberEntity output)
        {
            output = new MemberEntity
            {
                Id = Id,
                Created = Created,
                Updated = Updated,
                Active = Active,
                GivenName = GivenName,
                Surname = Surname,
                Mail = Mail,
                MailConfirmed = MailConfirmed,
                RenewPassword = RenewPassword,
                Role = Role
            };
        }

        public virtual void From(MemberEntity input)
        {
            Id = input.Id;
            Created = input.Created;
            Updated = input.Updated;
            Active = input.Active;
            GivenName = input.GivenName;
            Surname = input.Surname;
            Mail = input.Mail;
            MailConfirmed = input.MailConfirmed;
            RenewPassword = input.RenewPassword;
            Role = input.Role;
            
        }
    }
}