using System.Collections.Generic;
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
    public class CustomerController : Controller
    {
        private CustomerUseCase _customerUseCase;

        public CustomerController(CustomerUseCase customerUseCase)
        {
            _customerUseCase = customerUseCase;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Moderator,Admin")]
        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = "Kunden";
            return View();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpGet("/{controller}/api/list")]
        public async Task<ActionResult<IEnumerable<CustomerDataTransfer>>> GetList()
        {
            var r = await _customerUseCase.GetAllAsync();
            return r.To<CustomerDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPut("/{controller}/api")]
        public async Task<ActionResult<CustomerDataTransfer>> PutSingle([FromBody] CustomerDataTransfer customerDataTransfer)
        {
            if (customerDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            customerDataTransfer.To(out var customer);
            var r = await _customerUseCase.CreateSingleAsync(customer);

            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpGet("/{controller}/api/{id}")]
        public async Task<ActionResult<CustomerDataTransfer>> GetSingle(int id)
        {
            var r = await _customerUseCase.GetSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpPatch("/{controller}/api/{id}")]
        public async Task<ActionResult<CustomerDataTransfer>> PatchSingle(int id,
            [FromBody] CustomerDataTransfer customerDataTransfer)
        {
            if (customerDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            customerDataTransfer.To(out var customerEntity);
            var r = await _customerUseCase.UpdateSingleAsync(id, customerEntity);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "Admin,Moderator")]
        [HttpDelete("/{controller}/api/{id}")]
        public async Task<ActionResult<CustomerDataTransfer>> DeleteSingle(int id)
        {
            var r = await _customerUseCase.DeleteSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
    }
}