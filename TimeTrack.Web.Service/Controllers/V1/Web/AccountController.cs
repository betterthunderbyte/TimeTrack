using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTrack.Core.Model;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/{action}")]
    public class AccountController : Controller
    {
        private AccountUseCase _accountUseCase;
        private ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, AccountUseCase accountUseCase)
        {
            _logger = logger;
            _accountUseCase = accountUseCase;
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        [HttpGet()]
        public async Task<ViewResult> Index()
        {
            return View();
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Roles = "User,Moderator,Admin")]
        [HttpGet()]
        public async Task<ViewResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthenticationSchemes.Cookie);
            ViewBag.Title = "Logout";
            return View();
        }

        [HttpGet()]
        public ViewResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }
        
        [HttpPost()]
        public async Task<IActionResult> Login(string mail, string password)
        {
            var r = await _accountUseCase.ValidateLoginAsync(mail, password);
            
            if (r.Successful)
            {
                var role = "none";
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

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, r.Value.Mail),
                    new Claim(ClaimTypes.NameIdentifier, r.Value.Id.ToString()),
                    new Claim(ClaimTypes.Role, role),
                };

                await HttpContext.SignInAsync(AuthenticationSchemes.Cookie, new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));

                return RedirectToAction("Index", "Activity");
            }
            else
            {
                ViewBag.Error = r.MessageOutput;
            }
            ViewBag.Title = "Login";
            return View();
        }
        
        [HttpGet]
        public ViewResult Denied()
        {
            ViewBag.Title = "Access Denied";
            return View();
        }
    }
}