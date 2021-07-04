using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace TimeTrack.Core.Model
{
    public class MemberEntity : ITimestamp
    {
        /// <summary>
        /// 0 = Admin
        /// 1 = Moderator
        /// 2 = User
        /// </summary>
        public enum MemberRole
        {
            /// <summary>
            /// Admin
            /// </summary>
            Admin,
            /// <summary>
            /// Moderator
            /// </summary>
            Moderator,
            /// <summary>
            /// User
            /// </summary>
            User
        }
        
        
        private const int SaltLength = 16;
        private const int HashLength = 20;
        
        [Key]
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool Active { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public bool MailConfirmed { get; set; }

        public string Password { get; set; }
        public bool RenewPassword { get; set; }
        public MemberRole Role { get; set; }
        
        public List<ActivityEntity> Activities { get; set; }
        
        public void SetPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltLength]);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(HashLength);

            var hashBytes = new byte[HashLength + SaltLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);
            
            Password = Convert.ToBase64String(hashBytes);
        }
        
        public bool VerifyPassword(string password)
        {
            var hashBytes = Convert.FromBase64String(Password);
            var salt = new byte[SaltLength];
            Array.Copy(hashBytes, 0, salt, 0, SaltLength);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(HashLength);

            var success = true;

            for (var i = 0; i < HashLength; ++i)
            {
                if (hashBytes[i + SaltLength] != hash[i])
                {
                    success = false;
                    // Mit Absicht alle Zeichen prüfen, um Timing Attacks zu vermeiden
                }
            }
            
            return success;
        }
    }
}
