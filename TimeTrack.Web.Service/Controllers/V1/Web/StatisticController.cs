using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action=Index}")]
    public class StatisticController : Controller
    {
        private ActivityUseCase _activityUseCase;
        
        public StatisticController(ActivityUseCase activityUseCase)
        {
            _activityUseCase = activityUseCase;
        }
        
 
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Moderator,Admin,User")]
        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = "Statistik";
            return View();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Moderator,Admin,User")]
        [HttpGet("/{controller}/api/duration/from/week/in/days")]
        public async Task<ActionResult<WeekDurationDataTransfer>> GetDurationFromWeekInDaysFromOwner()
        {
            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                var r = await _activityUseCase.GetDurationFromWeekInDaysFromOwner(userId, DateTimeOffset.Now);
                return r.ToSingleAction();
            }
            
            return new BadRequestResult();
        }
        
        /*

        public async Task<ActionResult<ActivityDataTransfer>> GetAllBetweenAsync()
        {
            _activityUseCase.
        }*/
    }
}