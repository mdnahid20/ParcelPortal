using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPortal.Data;
using ParcelPortal.Models;
using System.Security.Claims;

namespace ParcelPortal.Controllers
{
    public class LoginController : Controller
    {
        public readonly ParcelPortalContext _context;

        public LoginController(ParcelPortalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var foundUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (foundUser != null)
                {
                    if (user.Password == foundUser.Password)
                    {
                        var foundRole = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == foundUser.Id);

                        if (foundRole == null)
                            return View(user);

                        var claims = new List<Claim>
                       {
                         new Claim(ClaimTypes.Email, foundUser.Email),
                         new Claim(ClaimTypes.Role, foundRole.Role)
                       };


                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                       new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid email.");
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
 }
