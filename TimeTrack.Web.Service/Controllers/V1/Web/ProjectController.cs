using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools.V1;
using TimeTrack.Web.Service.UseCase.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action=Index}")]
    public class ProjectController : Controller
    {
        private ProjectUseCase _projectUseCase;

        public ProjectController(ProjectUseCase projectUseCase)
        {
            _projectUseCase = projectUseCase;
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Moderator,Admin")]
        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = "Projekte";
            return View();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator,User")]
        [HttpGet("/{controller}/api/list")]
        public async Task<ActionResult<IEnumerable<ProjectDataTransfer>>> GetList()
        {
            var r = await  _projectUseCase.GetAllAsync();

            return r.To<ProjectDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPut("/{controller}/api")]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPatch("/{controller}/api/{id}")]
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
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpGet("/{controller}/api/{id}")]
        public async Task<ActionResult<ProjectDataTransfer>> GetSingle(int id)
        {
            var r = await _projectUseCase.GetSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpDelete("/{controller}/api/{id}")]
        public async Task<ActionResult<ProjectDataTransfer>> DeleteSingle(int id)
        {
            var r = await _projectUseCase.DeleteSingleAsync(id);
            return r.To<ProjectDataTransfer>().ToSingleAction();
        }
    }
}