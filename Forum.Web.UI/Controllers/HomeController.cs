using Forum.Web.UI.Clients.Authentication;
using Forum.Web.UI.Clients.Users;
using Forum.Web.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace Forum.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserClient _userClient;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUserClient userClient,
            IAuthenticationClient authenticationClient,
            ILogger<HomeController> logger)
        {
            _userClient = userClient;
            _authenticationClient = authenticationClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), model);
            }

            try
            {
                var user = await _authenticationClient
                    .LoginAsync(new AuthenticateDto
                    {
                        Username = model.Username,
                        Password = model.Password
                    });

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, user.Role.ToString()!),
                    new Claim(ClaimTypes.NameIdentifier, user.Username!),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()!),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ModelState.AddModelError("", "Invalid username or password");

                return View(nameof(Index), model);
            }

            return RedirectToAction(
                nameof(UsersController.IndexAsync),
                "Users");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
