﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TimeTrack.Models.V1
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
        public virtual MemberEntity Member { get; set; }
    }
}