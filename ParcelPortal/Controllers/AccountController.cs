using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ParcelPortal.Data;
using ParcelPortal.Models;
using ParcelPortal.ViewModels;
using System.Buffers;
using System.Diagnostics;

namespace ParcelPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public readonly ParcelPortalContext _context;
        private static string SearchValue { get; set; }
        private static string UserAttribute { get; set; } = "Name";
        private static int CurrentPage { get; set; } = 1;
        private static int totalAccount { get; set; }

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
            return Ok(new { UserAttribute = UserAttribute,SearchValue = SearchValue });
        }

        [HttpPost("{Controller}/PostSearchValue")]
        public IActionResult SearchAccount(string option, string value)
        {
            UserAttribute = option;
            SearchValue = value;

            return Ok(new { success = true });
        }

        [HttpGet("{controller}/GetPageNumber")]
        public IActionResult PageNumber()
        {
            
            int lastPage = (totalAccount + 9) / 10;
            int nextOne, nextTwo, previousOne, previousTwo;
            CurrentPage = Math.Min(CurrentPage, lastPage);
            CurrentPage = Math.Max(1, CurrentPage);

            nextOne = nextTwo = previousOne = previousTwo = -1;

            if (CurrentPage - 1 > 0)
                previousOne = CurrentPage - 1;
            if (CurrentPage - 2 > 0)
                previousTwo = CurrentPage - 2;

            if (CurrentPage + 1 <= lastPage)
                nextOne = CurrentPage + 1;
            if (CurrentPage + 2 <= lastPage)
                nextOne = CurrentPage + 2;

            return Ok(new { success = true, nextOne = nextOne, nextTwo = nextTwo, previousOne = previousOne, previousTwo = previousTwo });
        }

        [HttpPost("{controller}/PostPageNumber")]
        public IActionResult PageNumber(int page)
        {
            CurrentPage += page;
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

                if (userRole != null)
                {
                    UserAccount userAccount = new UserAccount();

                    userAccount.Name = user.Name;
                    userAccount.Email = user.Email;
                    userAccount.Password = user.Password;
                    userAccount.Role = userRole.Role;
                    userAccount.Id = user.Id;   

                    UserAccounts.Add(userAccount);
                }
            }


            if (SearchValue != null)
            {
                if (UserAttribute == "Name")
                {
                    UserAccounts = UserAccounts.Where(userAccount => userAccount.Name.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else if(UserAttribute == "Email")
                {
                    UserAccounts = UserAccounts.Where(userAccount => userAccount.Email.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else
                {
                    UserAccounts = UserAccounts.Where(userAccount => userAccount.Role.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
            }

            CurrentPage = Math.Max(1, CurrentPage); 
            totalAccount = UserAccounts.Count();
            CurrentPage = Math.Min((totalAccount + 9) / 10, CurrentPage);
            int takeAccount = Math.Min(totalAccount - (CurrentPage - 1) * 10, 10);
            int skipAccount = (CurrentPage - 1) * 10;
            var paggedAccounts = UserAccounts.Skip(skipAccount).Take(takeAccount);

            return Ok(paggedAccounts);
        }

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Account ID is required.");
            }

            var user = _context.User.FirstOrDefault(m => m.Id == id);
            var userRole = _context.UserRoles.FirstOrDefault(m => m.UserId == id);

            if (user == null || userRole == null)
            {
                return NotFound();
            }

            UserAccount userAccount = new UserAccount();

            userAccount.Id = user.Id;
            userAccount.Name = user.Name;
            userAccount.Email = user.Email;
            userAccount.Role = userRole.Role;
            userAccount.Password = user.Password;
            userAccount.OrderHistory = _context.Courier.Where(m => m.UserId == user.Id).ToList();

            return View(userAccount);
        }
    }
}