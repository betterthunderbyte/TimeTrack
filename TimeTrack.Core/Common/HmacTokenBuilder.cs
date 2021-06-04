using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace TimeTrack.Core.Common
{
    public class HmacTokenBuilder : TokenBuilder
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        
        public HmacTokenBuilder(string secret) : base(secret)
        {
        }

        public override TokenBuilder AddValue(string key, string value)
        {
            _values.Add(key, value);
            return this;
        }

        public override TokenBuilder AddValue(string key, int value)
        {
            _values.Add(key, value);
            return this;
        }

        public override TokenBuilder AddValue(string key, long value)
        {
            _values.Add(key, value);
            return this;
        }

        public override TokenBuilder AddValue(string key, bool value)
        {
            _values.Add(key, value);
            return this;
        }

        public override TokenBuilder AddValue(string key, float value)
        {
            _values.Add(key, value);
            return this;
        }

        public override TokenBuilder AddValue(string key, double value)
        {
            _values.Add(key, value);
            return this;
        }

        public override string Build()
        {
            string contentAsJson = JsonSerializer.Serialize(_values);
            var secretInBytes = Encoding.UTF8.GetBytes(Secret);
            
            using(HMACSHA256 hmac = new HMACSHA256(secretInBytes))
            {
                var contentAsBytes = Encoding.UTF8.GetBytes(contentAsJson);
                var hash = hmac.ComputeHash(contentAsBytes);

                WebEncoders.
                
                StringBuilder builder = new StringBuilder();
                builder.Append(contentAsBytes);
                builder.Append(".");
                builder.Append(hash);
            }
        }
    }
}