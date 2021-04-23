using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ApiCustomerController : ControllerBase
    {
        CustomerUseCase _customerUseCase;

        public ApiCustomerController(CustomerUseCase customerUseCase)
        {
            _customerUseCase = customerUseCase;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<CustomerDataTransfer>>> GetList()
        {
            var r = await _customerUseCase.GetAllAsync();
            return r.To<CustomerDataTransfer>().ToMultiAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPut]
        public async Task<ActionResult<CustomerDataTransfer>> PutSingle(CustomerDataTransfer customerDataTransfer)
        {
            if (customerDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            customerDataTransfer.To(out var customer);
            var r = await _customerUseCase.CreateSingleAsync(customer);

            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDataTransfer>> GetSingle(int id)
        {
            var r = await _customerUseCase.GetSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpPatch("{id}")]
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

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerDataTransfer>> DeleteSingle(int id)
        {
            var r = await _customerUseCase.DeleteSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
    }
}
