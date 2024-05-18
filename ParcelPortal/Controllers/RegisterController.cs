using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPortal.Data;
using ParcelPortal.Models;

namespace ParcelPortal.Controllers
{
    public class RegisterController : Controller
    {
        public readonly ParcelPortalContext _context;

        public RegisterController(ParcelPortalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var foundUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (foundUser == null)
                {
                    UserRoles userRole = new UserRoles();
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();

                    userRole.UserId = user.Id;
                    userRole.Role = "Customer";
                    _context.UserRoles.Add(userRole);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email is already used.");
                }
            }

            return View(user);
        }
    }
}
