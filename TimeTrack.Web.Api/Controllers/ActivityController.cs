using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private ActivityUseCase _activityUseCase;

        public ActivityController(ActivityUseCase activityUseCase)
        {
            _activityUseCase = activityUseCase;
        }
        
        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ActivityDataTransfer>>> GetAll()
        {
            var r = await _activityUseCase.GetAllAsync();
            return r.To<ActivityDataTransfer>().ToMultiAction();
        }
        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityDataTransfer>> GetSingle(int id)
        {
            var r = await _activityUseCase.GetSingleAsync(id);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [HttpPut]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityDataTransfer>> PutSingle([FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            activityDataTransfer.To(out var c);
            var r = await _activityUseCase.CreateSingleAsync(c);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityDataTransfer>> PatchSingle(int id,
            [FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            activityDataTransfer.To(out var c);
            var r = await _activityUseCase.UpdateSingleAsync(id, c);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ActivityDataTransfer>> DeleteSingle(int id)
        {
            var r = await _activityUseCase.DeleteSingleAsync(id);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [HttpGet("user/list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<IEnumerable<ActivityDataTransfer>>> GetAllFromUser()
        {
            if (User.ReceiveMemberId(out var userId))
            {
                var r = await _activityUseCase.GetAllFromUserAsync(userId);
                return r.To<ActivityDataTransfer>().ToMultiAction();
            }

            return Unauthorized();
        }
        
        [HttpPut("user")]  
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<ActivityDataTransfer>> PutSingleFromUser(
            [FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }

            if (User.ReceiveMemberId(out var userId))
            {
                activityDataTransfer.OwnerFk = userId;
                activityDataTransfer.To(out var activityEntity);
                var r = await _activityUseCase.CreateSingleAsync(activityEntity);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return Unauthorized();
        }
        
        [HttpPatch("user/{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<ActivityDataTransfer>> PatchSingleFromUser(int id, [FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }

            if (User.ReceiveMemberId(out var userId))
            {
                activityDataTransfer.To(out var activityEntity);
                
                var r = await _activityUseCase.UpdateSingleFromUserAsync(userId, id, activityEntity);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }

            return Unauthorized();
        }
        
        [HttpDelete("user/{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<ActivityDataTransfer>> DeleteSingleFromUser(int id)
        {
            if (User.ReceiveMemberId(out var userId))
            {
                var r = await _activityUseCase.DeleteSingleFromUserAsync(userId, id);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return Unauthorized();
        }
    }
}
