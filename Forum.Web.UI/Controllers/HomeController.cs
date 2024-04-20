using Forum.Web.UI.Clients.Authentication;
using Forum.Web.UI.Clients.Users;
using Forum.Web.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Forum.Application.Dto;
using Forum.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using AuthenticateDto = Forum.Web.UI.Clients.Authentication.AuthenticateDto;


namespace Forum.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserClient _userClient;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;



        public HomeController(
            IUserClient userClient,
            IAuthenticationClient authenticationClient,
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _userClient = userClient;
            _authenticationClient = authenticationClient;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5038/"); // Base URL of API
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeAuthenticated));
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
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [Authorize (Roles = "User, Admin")]
        public IActionResult HomeAuthenticated()
        {
            var model = new TopicsView
            {
                Topics = new List<Topic>()
            };
            return View(model);
            
        }

        public IActionResult Register()
        {
            return View();
        }
        
        public IActionResult Logout()
        {
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
            try
            {
                // Deserialize the form collection into CreateUserDto
                var userDto = new CreateUserDto
                {
                    // Populate properties from form collection
                    // Example:
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Email = collection["Email"],
                    Username = collection["Username"],
                    Password = collection["Password"],
                    ConfirmPassword = collection["ConfirmPassword"]
                    // Map other properties accordingly
                };

                // Serialize CreateUserDto to JSON


                // Send POST request to API endpoint
                var response = await _httpClient.PostAsync($"api/Users/{collection["Role"]}", new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                // Handle success
                // Example: redirect to a success page
                return RedirectToAction(nameof(Index));

            }
            catch (Exception e)
            {
                // Handle exception
                // Example: log the exception
                Console.WriteLine(e);
                // Return a view with error message
                return View("Error");
            }
        }
    }
}
