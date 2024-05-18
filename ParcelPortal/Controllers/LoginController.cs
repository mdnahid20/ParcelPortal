using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPortal.Data;
using ParcelPortal.Models;

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
                        RedirectToAction("Home", "Index");
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
    }
}
