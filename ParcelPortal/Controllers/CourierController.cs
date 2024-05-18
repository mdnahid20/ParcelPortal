using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;
using ParcelPortal.Models;
using System.Diagnostics.CodeAnalysis;

namespace ParcelPortal.Controllers
{
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
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index([Bind("Id,SenderName,SenderEmail,SenderPhone,SenderBranch,SenderAddress,RecevierName,RecevierEmail,RecevierPhone,ReceiverBranch,RecevierAddress,ProductQuantity,DeliveryTime,ProductPrice")] Courier Courier)
        {
            Courier.DeliveryTime = DateTime.UtcNow.AddDays(2).ToString();
            Courier.ProductPrice = (20 + Courier.ProductQuantity * 10).ToString();
           
            if (ModelState.IsValid) 
            {
                _context.Courier.Add(Courier);
                await _context.SaveChangesAsync();   
                return RedirectToAction("Index","Home");   
            }

            return View(Courier);
        }
    }
}
