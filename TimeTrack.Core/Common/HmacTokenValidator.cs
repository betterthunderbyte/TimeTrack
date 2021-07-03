using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace TimeTrack.Core.Common
{
    public class HmacTokenValidator : TokenValidator
    {
        private bool _isValid = false;
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        
        public HmacTokenValidator(string secret) : base(secret)
        {
        }

        public override TokenValidator ValidateToken(string token)
        {
            if (!token.Contains("."))
            {
                return this;
            }

            var parts = token.Split(".");

            var dataInBytes = WebEncoders.Base64UrlDecode(parts[0]);
            var hashInBytes = WebEncoders.Base64UrlDecode(parts[1]);
            
            using(HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret)))
            {
                var hash = hmac.ComputeHash(dataInBytes);
                _isValid = true;
                for (int i = 0; i < hash.Length; ++i)
                {
                    if (hash[i] != hashInBytes[i])
                    {
                        _isValid = false;
                    }
                }
                
            }

            _values = JsonSerializer.Deserialize<Dictionary<string, object>>(dataInBytes);

            return this;
        }

        public override bool IsValid()
        {
            return _isValid;
        }


        public override bool GetValue(string key, out string value)
        {
            value = "";
            if (_values.ContainsKey(key))
            {
                value = (string)_values[key];
                return true;
            }

            return false;
        }

        public override bool GetValue(string key, out int value)
        {
            value = 0;
            if (_values.ContainsKey(key))
            {
                value = (int)_values[key];
                return true;
            }

            return false;
        }

        public override bool GetValue(string key, out long value)
        {
            value = 0;
            if (_values.ContainsKey(key))
            {
                value = (long)_values[key];
                return true;
            }

            return false;
        }

        public override bool GetValue(string key, out bool value)
        {
            value = false;
            if (_values.ContainsKey(key))
            {
                value = (bool)_values[key];
                return true;
            }

            return false;
        }

        public override bool GetValue(string key, out float value)
        {
            value = 0;
            if (_values.ContainsKey(key))
            {
                value = (float)_values[key];
                return true;
            }

            return false;
        }

        public override bool GetValue(string key, out double value)
        {
            value = 0;
            if (_values.ContainsKey(key))
            {
                value = (double)_values[key];
                return true;
            }

            return false;
        }
    }
}