using System.Linq;
using System.Security.Claims;

namespace TimeTrack.Web.Api.Common
{
    public static class ClaimsPrincipalExtension
    {
        public static bool ReceiveMemberId(this ClaimsPrincipal claimsPrincipal, out int id)
        {
            id = 0;
            
            var claim = claimsPrincipal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            if (int.TryParse(claim.Value, out id))
            {
                return true;
            }

            return false;
        }
    }
}