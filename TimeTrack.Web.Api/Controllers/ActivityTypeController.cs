using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using TimeTrack.Core.DataTransfer;

using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;


namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class ActivityTypeController : ControllerBase
    {
        private ActivityTypeUseCase _activityTypeUseCase;
        
        public ActivityTypeController(ActivityTypeUseCase activityTypeUseCase)
        {
            _activityTypeUseCase = activityTypeUseCase;
        }
        
        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [SwaggerOperation(Summary = "Gibt alle Aktivitätstypen zurück", Description = "Ist für alle verfügbar.")]
        [SwaggerResponse(statusCode:200)]
        [SwaggerResponse(statusCode:401, Description = "Wenn der Nutzer nicht angemeldet ist.", Type = null)]
        public async Task<ActionResult<IEnumerable<ActivityTypeDataTransfer>>> GetAll()
        {
            var r = await _activityTypeUseCase.GetAllAsync();
            return r.To<ActivityTypeDataTransfer>().ToMultiAction();
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
  
        public async Task<ActionResult<ActivityTypeDataTransfer>> PutSingle(ActivityTypeDataTransfer activityTypeDataTransfer)
        {
            if (activityTypeDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            activityTypeDataTransfer.To(out var activityTypeEntity);
            var r = await _activityTypeUseCase.CreateSingleAsync(activityTypeEntity);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }
        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> GetSingle(int id)
        {
            var r = await _activityTypeUseCase.GetSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }
        
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
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
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> DeleteSingle(int id)
        {
            var r = await _activityTypeUseCase.DeleteSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }
        
    }
}
