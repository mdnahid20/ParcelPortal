using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParcelPortal.Data;
using ParcelPortal.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ParcelPortal.Controllers
{
    [Authorize]
    public class CourierController : Controller
    {
        public readonly ParcelPortalContext _context;
        public CourierController(ParcelPortalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/GetBranch")]
        public IActionResult Branch() 
        {
            var branch = _context.Branch.ToList();
            return Ok(branch);  
        }

        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index([Bind("Id,SenderName,SenderEmail,SenderPhone,SenderBranch,SenderAddress,ReceiverName,ReceiverEmail,ReceiverPhone,ReceiverBranch,ReceiverAddress,ProductQuantity,DeliveryTime,ProductPrice")] Courier Courier)
        {
            if (ModelState.IsValid) 
            {
                Courier.DeliveryTime = DateTime.UtcNow.AddDays(2).ToString();
                Courier.ProductPrice = (20 + Courier.ProductQuantity * 10).ToString();
                Courier.ConsignmentNumber = GenerateConsignmentNumber();
                Courier.Status = ((ProductStatus)1).ToString();

                var emailClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

                if (emailClaim != null)
                {
                    var user = _context.User.FirstOrDefault(x => x.Email == emailClaim.Value);

                    if (user != null)
                    {
                        Courier.UserId = user.Id;
                    }else
                        return View(Courier);
                }

                _context.Courier.Add(Courier);
                await _context.SaveChangesAsync();   
                return RedirectToAction("Index","Home");   
            }

            return View(Courier);
        }

        public string GenerateConsignmentNumber()
        {
            string format = "YYMM-####-PRD";
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");

            int newNumber = _context.Courier.Count() + 1;

            string consignmentNumber = format.Replace("#", newNumber.ToString("D4"))
                                            .Replace("YY", year)
                                            .Replace("MM", month);

            return consignmentNumber;
        }
    }
}
