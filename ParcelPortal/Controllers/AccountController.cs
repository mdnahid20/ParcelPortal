using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ParcelPortal.Data;
using ParcelPortal.Models;
using ParcelPortal.ViewModels;
using System.Buffers;
using System.Diagnostics;

namespace ParcelPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public readonly ParcelPortalContext _context;
        private static string SearchValue { get; set; }

        private static string UserAttribute { get; set; } = "Name";

        private static string UserRole { get; set; } = "All";

        public AccountController(ILogger<AccountController> logger, ParcelPortalContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/GetSearchValue")]
        public IActionResult SearchAccount()
        {
            return Ok(new { UserAttribute = UserAttribute , UserRole = UserRole , SearchValue = SearchValue });
        }

        [HttpPost("{Controller}/PostSearchValue")]
        public IActionResult SearchAccount(string userAttribute,string userRole,string searchValue)
        {
            UserAttribute = userAttribute;
            UserRole = userRole;
            SearchValue = searchValue;

            return Ok(new { success = true });
        }

        [HttpGet("{Controller}/GetAccount")]
        public IActionResult GetAccountList()
        {
            var users = _context.User.ToList();
            var userRoles = _context.UserRoles.ToList();
            List<UserAccount> UserAccounts = new List<UserAccount>();

            foreach (var user in users) 
            {
                var userRole = userRoles.FirstOrDefault(x => x.UserId == user.Id); 
                
                if(userRole != null)
                {
                    UserAccount userAccount = new UserAccount();
                    
                    userAccount.Name = user.Name;   
                    userAccount.Email = user.Email;
                    userAccount.Password = user.Password;
                    userAccount.Role = userRole.Role;

                    UserAccounts.Add(userAccount);
                }
            }

            if(UserRole != "All")
            {
                UserAccounts = UserAccounts.Where(userAccount => userAccount.Role == UserRole).ToList();
            }

            if(SearchValue != null)
            {
                if (UserAttribute == "Name")
                {
                    UserAccounts = UserAccounts.Where(userAccount => userAccount.Name.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else 
                {
                    UserAccounts = UserAccounts.Where(userAccount => userAccount.Email.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
            }

            return Ok(UserAccounts);
        }

        /*

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Account ID is required.");
            }

            var Account = Account.FirstOrDefault(m => m.Id == id);

            if (Account == null)
            {
                return NotFound();
            }

            return View(Account);
        }*/
    }
}