using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;

namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action=Index}")]
    public class ActivityTypeController : Controller
    {
        private ActivityTypeUseCase _activityTypeUseCase;
        private ILogger<ActivityTypeController> _logger;

        public ActivityTypeController(ILogger<ActivityTypeController> logger, ActivityTypeUseCase activityTypeUseCase)
        {
            _logger = logger;
            _activityTypeUseCase = activityTypeUseCase;
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Moderator,Admin")]
        [HttpGet]
        public ViewResult Index()
        {

            ViewBag.Title = "Aktivitätentypen";
            return View();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpGet("/{controller}/api/list")]
        public async Task<ActionResult<IEnumerable<ActivityTypeDataTransfer>>> GetAll()
        {

            var r = await _activityTypeUseCase.GetAllAsync();
            return r.To<ActivityTypeDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPut("/{controller}/api")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> PutSingle([FromBody]ActivityTypeDataTransfer activityTypeDataTransfer)
        {

            if (activityTypeDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            activityTypeDataTransfer.To(out var activityTypeEntity);
            var r = await _activityTypeUseCase.CreateSingleAsync(activityTypeEntity);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpGet("/{controller}/api/{id}")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> GetSingle(int id)
        {

            var r = await _activityTypeUseCase.GetSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPatch("/{controller}/api/{id}")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> PatchSingle(int id,
            [FromBody] ActivityTypeDataTransfer activityTypeDataTransfer)
        {

            if (activityTypeDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            activityTypeDataTransfer.To(out var activityTypeEntity);
            var r = await _activityTypeUseCase.UpdateSingleAsync(id, activityTypeEntity);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpDelete("/{controller}/api/{id}")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> DeleteSingle(int id)
        {
            var r = await _activityTypeUseCase.DeleteSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }
        
    }
}