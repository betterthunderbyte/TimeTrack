﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeTrack.Core;
using TimeTrack.Core.Configuration;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Core.Model;
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [ApiController, Route("v1/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountUseCase _accountUseCase;
        private IOptions<JsonWebTokenConfiguration> _configuration;

        public AccountController(IAccountUseCase accountUseCase, IOptions<JsonWebTokenConfiguration> configuration)
        {
            _accountUseCase = accountUseCase;
            _configuration = configuration;
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

            if (r.Successful)
            {
                string role = "none";
                var member = r.Value;
                switch (r.Value.Role)
                {
                    case MemberEntity.MemberRole.Admin:
                        role = "Admin";
                        break;
                    case MemberEntity.MemberRole.Moderator:
                        role = "Moderator";
                        break;
                    case MemberEntity.MemberRole.User:
                        role = "User";
                        break;
                    default:
                        break;
                }
            
                var securityKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration.Value.Secret)
                );
                var credentials = new SigningCredentials(
                    securityKey, 
                    SecurityAlgorithms.HmacSha256
                );
                var token = new JwtSecurityToken(
                    _configuration.Value.Issuer, 
                    _configuration.Value.Audience, 
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Surname, member.Surname),
                        new Claim(ClaimTypes.GivenName, member.GivenName),
                        new Claim(ClaimTypes.Email, member.Mail),
                    }, 
                    null, 
                    DateTime.Now.AddSeconds(_configuration.Value.AccessTokenExpiration), 
                    credentials);
            
                return UseCaseResult<NewTokenDataTransfer>.Success(new NewTokenDataTransfer()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                }).ToSingleAction();
            }
            
            return UseCaseResult<NewTokenDataTransfer>.Failure(UseCaseResultType.BadRequest, null).ToSingleAction();
        }
 
                
        [AllowAnonymous]
        [HttpGet("confirm")]
        public async Task<ActionResult> ConfirmAccount()
        {
            return new OkResult();
        }
        
        [AllowAnonymous]
        [HttpPost("password/reset")]
        public async Task<ActionResult> ResetPassword()
        {
            return new OkResult();
        }

        [HttpPost("password/change")]
        public async Task<ActionResult> ChangePassword()
        {
            return new OkResult();
        }
  
        [HttpPatch()]
        public async Task<ActionResult> Update()
        {
            return new OkResult();
        }

        [HttpDelete()]
        public async Task<ActionResult> Delete()
        {
            return new OkResult();
        }
    }
}
