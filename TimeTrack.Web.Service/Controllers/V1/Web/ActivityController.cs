using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;

// ToDo(Thorben) Im Frontend wird das Datum falsch verarbeitet 
namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action=Index}")]
    public class ActivityController : Controller
    {
        private ActivityUseCase _activityUseCase;
        private OtherUseCase _otherUseCase;
        private ILogger<ActivityController> _logger;

        public ActivityController(ILogger<ActivityController> logger, ActivityUseCase activityUseCase, OtherUseCase otherUseCase)
        {
            _logger = logger;
            _activityUseCase = activityUseCase;
            _otherUseCase = otherUseCase;
        }

        [Route("/")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie)]
        [HttpGet]
        public IActionResult AutoRedirect()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction(controllerName: "Account", actionName: "Login");
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        [HttpGet]
        public ViewResult Index()
        {

            ViewBag.Title = "Aktivitäten";
            return View();
        }
        
        [HttpGet("/{controller}/api/full")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<FullDataTransfer>> GetFullData()
        {

            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                var r = await _otherUseCase.GetFullFromUserAsync(userId);
                return r.ToSingleAction();
            }
            
            return new BadRequestResult();
        }

        [HttpPut("/{controller}/api")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<ActivityDataTransfer>> CreateSingle([FromBody]ActivityDataTransfer activityDataTransfer)
        {

            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                activityDataTransfer.To(out var activityEntity);
                activityEntity.OwnerFk = userId;
                var r = await _activityUseCase.CreateSingleAsync(activityEntity);

                return r.To<ActivityDataTransfer>().ToSingleAction();
            }

            return new BadRequestResult();
        }
        
        [HttpPatch("/{controller}/api/{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<ActivityDataTransfer>> PatchSingle(int id, [FromBody] ActivityDataTransfer activityDataTransfer)
        {

            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            
            if(claim != null && int.TryParse(claim.Value, out userId))
            {
                activityDataTransfer.To(out var activityEntity);
                var r = await _activityUseCase.UpdateSingleFromUserAsync(userId, id, activityEntity);

                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return new UnauthorizedResult();
        }
        
        [HttpDelete("/{controller}/api/{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        public async Task<ActionResult<ActivityDataTransfer>> DeleteSingle(int id)
        {
            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            
            if(claim != null && int.TryParse(claim.Value, out userId))
            {
                var r = await _activityUseCase.DeleteSingleFromUserAsync(userId, id);

                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return new UnauthorizedResult();
        }
    }
}