using TimeTrack.Core.Model;

namespace TimeTrack.Core.DataTransfer
{
    public class MemberWithPasswordDataTransfer : MemberDataTransfer
    {
        public string Password { get; set; }
        
        public override void To(out MemberEntity output)
        {
            base.To(out output);
            output.SetPassword(Password);
        }
    }
}