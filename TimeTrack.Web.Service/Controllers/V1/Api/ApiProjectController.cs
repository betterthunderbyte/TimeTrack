using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ApiProjectController : ControllerBase
    {
        ProjectUseCase _projectUseCase;
        private ILogger<ApiProjectController> _logger;

        public ApiProjectController(ProjectUseCase projectUseCase, ILogger<ApiProjectController> logger)
        {
            _projectUseCase = projectUseCase;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ProjectDataTransfer>>> GetList()
        {
            var r = await  _projectUseCase.GetAllAsync();

            return r.To<ProjectDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPut()]
        public async Task<ActionResult<ProjectDataTransfer>> PutSingle([FromBody]ProjectDataTransfer projectDataTransfer)
        {
            if (projectDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            projectDataTransfer.To(out var c);
            var r = await _projectUseCase.CreateSingleAsync(c);

            return r.To<ProjectDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPatch]
        public async Task<ActionResult<ProjectDataTransfer>> PatchSingle(int id,
            [FromBody] ProjectDataTransfer projectDataTransfer)
        {
            if (projectDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            projectDataTransfer.To(out var p);
            var updatedProject = await _projectUseCase.UpdateSingleAsync(id, p);
            return updatedProject.To<ProjectDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDataTransfer>> GetSingle(int id)
        {
            var r = await _projectUseCase.GetSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectDataTransfer>> DeleteSingle(int id)
        {
            var r = await _projectUseCase.DeleteSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }
    }
}
