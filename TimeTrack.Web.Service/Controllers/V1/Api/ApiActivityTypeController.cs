using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
 using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools.V1;
 using TimeTrack.Web.Service.UseCase.V1;

 namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ApiActivityTypeController : ControllerBase
    {
        private ActivityTypeUseCase _activityTypeUseCase;
        
        public ApiActivityTypeController(ActivityTypeUseCase activityTypeUseCase)
        {
            _activityTypeUseCase = activityTypeUseCase;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ActivityTypeDataTransfer>>> GetAll()
        {
            var r = await _activityTypeUseCase.GetAllAsync();
            return r.To<ActivityTypeDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPut]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> GetSingle(int id)
        {
            var r = await _activityTypeUseCase.GetSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPatch("{id}")]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActivityTypeDataTransfer>> DeleteSingle(int id)
        {
            var r = await _activityTypeUseCase.DeleteSingleAsync(id);
            return r.To<ActivityTypeDataTransfer>().ToSingleAction();
        }
        
    }
}
