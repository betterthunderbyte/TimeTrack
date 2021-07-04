using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;

namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action=Index}")]
    public class AdministrationController : Controller
    {
        private MemberUseCase _memberUseCase;
        
        public AdministrationController(MemberUseCase memberUseCase)
        {
            _memberUseCase = memberUseCase;
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin")]
        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = "Administration";
            return View();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin")]
        [HttpGet("/[controller]/api/member/list")]
        public async Task<ActionResult<IEnumerable<MemberDataTransfer>>> GetAll()
        {
            var r = await _memberUseCase.GetAllAsync();
            return r.To<MemberDataTransfer>().ToMultiAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin")]
        [HttpPatch("/[controller]/api/member/{id}")]
        public async Task<ActionResult<MemberDataTransfer>> PatchSingle(int id, [FromBody] MemberDataTransfer memberDataTransfer)
        {
            memberDataTransfer.To(out var memberEntity);

            var r = await _memberUseCase.UpdateSingleAsync(id, memberEntity);
            return r.To<MemberDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin")]
        [HttpPut("/[controller]/api/member")]
        public async Task<ActionResult<MemberDataTransfer>> PutSingle([FromBody] MemberWithPasswordDataTransfer memberDataTransfer)
        {
            memberDataTransfer.To(out var memberEntity);

            var r = await _memberUseCase.PutSingleAsync(memberEntity);
            return r.To<MemberDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin")]
        [HttpDelete("/[controller]/api/member/{id}")]
        public async Task<ActionResult<IEnumerable<MemberDataTransfer>>> DeleteSingle(int id)
        {
            var r = await _memberUseCase.DeleteSingleAsync(id);
            return r.To<MemberDataTransfer>().ToMultiAction();
        }
    }
}