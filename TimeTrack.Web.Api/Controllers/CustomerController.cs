using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;


namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomerUseCase _customerUseCase;

        public CustomerController(ICustomerUseCase customerUseCase)
        {
            _customerUseCase = customerUseCase;
        }
        
        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator,User")]
        public async Task<ActionResult<IEnumerable<CustomerDataTransfer>>> GetList()
        {
            var r = await _customerUseCase.GetAllAsync();
            return r.To<CustomerDataTransfer>().ToMultiAction();
        }
        
        [HttpPut]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
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
        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<CustomerDataTransfer>> GetSingle(int id)
        {
            var r = await _customerUseCase.GetSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
        
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
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
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin,Moderator")]
        public async Task<ActionResult<CustomerDataTransfer>> DeleteSingle(int id)
        {
            var r = await _customerUseCase.DeleteSingleAsync(id);
            return r.To<CustomerDataTransfer>().ToSingleAction();
        }
    }
}
