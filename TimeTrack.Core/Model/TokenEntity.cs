using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Core.Model
{
    public class TokenEntity : ITimestamp
    {
        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }

        public DateTimeOffset ExpirationTime { get; set; }

        public int MemberFk { get; set; }
        public MemberEntity Member { get; set; }
    }
}
