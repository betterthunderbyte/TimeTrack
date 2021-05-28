using TimeTrack.Core.DataTransfer;
using Xunit;

namespace TimeTrack.Core.UnitTest
{
    public class LoginDataTransferTest
    {
        [Fact]
        public void CheckFields()
        {
            LoginDataTransfer loginDataTransfer = new LoginDataTransfer()
            {
                Mail = " test@test.de",
                Password = "test"
            };
            Assert.Equal("test@test.de", loginDataTransfer.Mail);

            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test@test.de ",
                Password = "test"
            };
            Assert.Equal("test@test.de", loginDataTransfer.Mail);
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = " test@test.de ",
                Password = "test"
            };
            Assert.Equal("test@test.de", loginDataTransfer.Mail);

        }
        
        [Fact]
        public void CheckIsValid()
        {
            LoginDataTransfer loginDataTransfer = new LoginDataTransfer();
            Assert.False(loginDataTransfer.IsValid());
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = ""
            };
            Assert.False(loginDataTransfer.IsValid());
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Password = ""
            };
            Assert.False(loginDataTransfer.IsValid());

            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "",
                Password = ""
            };
            Assert.False(loginDataTransfer.IsValid());
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test",
                Password = "test"
            };
            Assert.False(loginDataTransfer.IsValid());
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test@test",
                Password = "test"
            };
            Assert.False(loginDataTransfer.IsValid());

            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test.de",
                Password = "test"
            };
            Assert.False(loginDataTransfer.IsValid());

            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test@test.de",
                Password = "test"
            };
            Assert.True(loginDataTransfer.IsValid());
            
            loginDataTransfer = new LoginDataTransfer()
            {
                Mail = "test@",
                Password = "test"
            };
            Assert.False(loginDataTransfer.IsValid());

 
        }
    }
}