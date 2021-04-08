using TimeTrack.Models.V1;

namespace TimeTrack.Core.DataTransfer.V1
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