using AspNetCoreBasicAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCoreBasicAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login(LoginUser User)
        {
            // user validation logic
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.UserName),
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim("Department","Hr"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "20/12/2021")
            };
            var identity = new ClaimsIdentity(claims, "SmartCookies");
            var principle = new ClaimsPrincipal(identity);

            var authenticatioProperties = new AuthenticationProperties()
            {
                                                                                                                        
            };

            await HttpContext.SignInAsync("SmartCookies", principle);
            return LocalRedirect(User.ReturnUrl);
        }
        public IActionResult LoginPost(string returnUrl)
        {
            LoginUser loginUser = new LoginUser()
            {
                ReturnUrl = returnUrl
            };
            return View(loginUser);
        }
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}