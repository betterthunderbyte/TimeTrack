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
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ApiActivityController : ControllerBase
    {
        private ActivityUseCase _activityUseCase;

        public ApiActivityController(ActivityUseCase activityUseCase)
        {
            _activityUseCase = activityUseCase;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ActivityDataTransfer>>> GetAll()
        {
            var r = await _activityUseCase.GetAllAsync();
            return r.To<ActivityDataTransfer>().ToMultiAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDataTransfer>> GetSingle(int id)
        {
            var r = await _activityUseCase.GetSingleAsync(id);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPut]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPatch("{id}")]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActivityDataTransfer>> DeleteSingle(int id)
        {
            var r = await _activityUseCase.DeleteSingleAsync(id);
            return r.To<ActivityDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpGet("user/list")]
        public async Task<ActionResult<IEnumerable<ActivityDataTransfer>>> GetAllFromUser()
        {
            var userId = 0;
            var claim = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);
            
            if(claim != null && int.TryParse(claim.Value, out userId))
            {
                var r = await _activityUseCase.GetAllFromUserAsync(userId);
                return r.To<ActivityDataTransfer>().ToMultiAction();
            }

            return Unauthorized();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpPut("user")]
        public async Task<ActionResult<ActivityDataTransfer>> PutSingleFromUser(
            [FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            var userId = 0;
            var claim = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                activityDataTransfer.OwnerFk = userId;
                activityDataTransfer.To(out var activityEntity);
                var r = await _activityUseCase.CreateSingleAsync(activityEntity);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return Unauthorized();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpPatch("user/{id}")]
        public async Task<ActionResult<ActivityDataTransfer>> PatchSingleFromUser(int id, [FromBody] ActivityDataTransfer activityDataTransfer)
        {
            if (activityDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            var claim = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            var userId = 0;
            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                activityDataTransfer.To(out var activityEntity);
                
                var r = await _activityUseCase.UpdateSingleFromUserAsync(userId, id, activityEntity);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return Unauthorized();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpDelete("user/{id}")]
        public async Task<ActionResult<ActivityDataTransfer>> DeleteSingleFromUser(int id)
        {
            var claim = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            var userId = 0;
            if (claim != null && int.TryParse(claim.Value, out userId))
            {
                var r = await _activityUseCase.DeleteSingleFromUserAsync(userId, id);
                return r.To<ActivityDataTransfer>().ToSingleAction();
            }
            
            return Unauthorized();
        }
    }
}
