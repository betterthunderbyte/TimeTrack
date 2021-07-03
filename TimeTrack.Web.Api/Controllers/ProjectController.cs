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
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class ProjectController : ControllerBase
    {
        IProjectUseCase _projectUseCase;
        private ILogger<ProjectController> _logger;

        public ProjectController(IProjectUseCase projectUseCase, ILogger<ProjectController> logger)
        {
            _projectUseCase = projectUseCase;
            _logger = logger;
        }
        
        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<IEnumerable<ProjectDataTransfer>>> GetList()
        {
            var r = await  _projectUseCase.GetAllAsync();

            return r.To<ProjectDataTransfer>().ToMultiAction();
        }
        
        [HttpPut()]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
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
        
        [HttpPatch]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
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
        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ProjectDataTransfer>> GetSingle(int id)
        {
            var r = await _projectUseCase.GetSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<ProjectDataTransfer>> DeleteSingle(int id)
        {
            var r = await _projectUseCase.DeleteSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }
    }
}
