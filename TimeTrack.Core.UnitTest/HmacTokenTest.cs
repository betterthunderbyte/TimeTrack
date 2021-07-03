using TimeTrack.Core.Common;
using Xunit;

namespace TimeTrack.Core.UnitTest
{
    public class HmacTokenTest
    {
        [Fact]
        public void TokenValidationTest()
        {
            HmacTokenServiceFactory factory =
                new HmacTokenServiceFactory(new TokenConfiguration() {Secret = "SuperSecret"});

            var tokenBuilder = factory.CreateTokenBuilder();
            var token = tokenBuilder.AddValue("id", 10).Build();

            var validator = factory.CreateTokenValidator();
            validator.ValidateToken(token);
            Assert.True(validator.IsValid());
            Assert.True(validator.GetValue("id", out int v));
            Assert.Equal(10, v);
        }
    }
}