using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.UseCase.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [ApiController, Route("v1/api/[controller]")]
    public class ApiAccountController : ControllerBase
    {
        private AccountUseCase _accountUseCase;

        public ApiAccountController(AccountUseCase accountUseCase)
        {
            _accountUseCase = accountUseCase;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<NewTokenDataTransfer>> Login([FromBody] LoginDataTransfer loginDataTransfer)
        {
            if (loginDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            var r = await _accountUseCase.LoginAsync(loginDataTransfer);
            return r.ToSingleAction();
        }
        
    }
}
