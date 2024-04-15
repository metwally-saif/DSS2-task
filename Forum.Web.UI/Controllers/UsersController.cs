using Forum.Application.Dto;
using Forum.Web.UI.Clients.Users;
using Forum.Web.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


namespace Forum.Web.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5038/"); // Base URL of API
        }

        // GET: UsersContrller
        public async Task<ActionResult> IndexAsync()
        {
            var response = await _httpClient.GetAsync("api/Users"); // API endpoint URL
            if (response.IsSuccessStatusCode)
            {

                var users = await response.Content.ReadAsAsync<IEnumerable<UserShortViewModel>>();
                return View(users);

            }
            else
            {
                // Handle API error response
                return View(Enumerable.Empty<UserShortViewModel>());
            }
        }

        // GET: UsersContrller/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsAsync<UserDetailsViewModel>();

                    // Assuming UserDetailsViewModel is a single user details view model,
                    // you might want to select the first item if you expect a single user

                    if (user != null)
                    {
                        return View(user); // Pass the UserDetailsViewModel to the view
                    }
                    else
                    {
                        // Handle API response with no user found
                        return View("NotFound"); // You can create a NotFound.cshtml view for this purpose
                    }
                }
                else
                {
                    // Handle API error response
                    return View("Error"); // You can create an Error.cshtml view for this purpose
                }
            }
            catch (Exception e)
            {
                // Handle exception (e.g., log error)
                return View("Error"); // You can create an Error.cshtml view for this purpose
            }
        }

        // GET: UsersContrller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersContrller/Create
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
                var response = await _httpClient.PostAsync("api/Users", new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (!response.IsSuccessStatusCode)
                {
                    return View();
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

            // GET: UsersContrller/Edit/{id}
            public async Task<ActionResult> EditAsync(int id, UpdateUserViewModel mode)
        {
           try
            {
                if (mode.Email == null)
                {

                    var response = await _httpClient.GetAsync($"api/Users/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var user = await response.Content.ReadAsAsync<UpdateUserViewModel>();
                        return View(user);
                    }
                    else
                    {
                        // Handle API error response
                        return View();
                    }
                }
                       else
                {
                    var response = await _httpClient.PutAsync($"api/Users/{id}", new StringContent(JsonConvert.SerializeObject(mode), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle API error response
                        return View();
                    }
                }   
            } catch (Exception e)
            {
                return View();
            }
            
        }

        // POST: UsersContrller/Delete/{id}
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle API error response
                    // For example, you could log the error or display a message to the user
                    ModelState.AddModelError(string.Empty, "Failed to delete user. Please try again.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                // Handle exception
                // For example, log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
