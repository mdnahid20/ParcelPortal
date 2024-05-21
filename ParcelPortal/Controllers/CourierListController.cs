using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;
using ParcelPortal.Models;
using ParcelPortal.ViewModels;
using System.Security.Claims;

namespace ParcelPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourierListController : Controller
    {
        public readonly ParcelPortalContext _context;
        private static string SearchValue { get; set; }
        private static string CourierAttribute { get; set; } = "ConsignmentNumber";
        private static int CurrentPage { get; set; } = 1;
        private static int totalCourierList { get; set; }
        private static int? Id { get; set; }
        public CourierListController(ParcelPortalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/GetSearchValue")]
        public IActionResult SearchCourier()
        {
            return Ok(new { value = SearchValue, option = CourierAttribute });
        }

        [HttpPost("{Controller}/PostSearchValue")]
        public IActionResult SearchCourier(string option, string value)
        {
            CourierAttribute = option;
            SearchValue = value;

            return Ok(new { success = true });
        }

        [HttpGet("{controller}/GetPageNumber")]
        public IActionResult PageNumber()
        {
            int lastPage = (totalCourierList + 9) / 10;
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

        [HttpGet("{Controller}/GetCourierList")]
        public IActionResult GetCourierList()
        {
            List<ShortCourier> shortCouriers = new List<ShortCourier>();
            var couriers = _context.Courier.ToList();

            foreach (var courier in couriers)
            {
                ShortCourier shortCourier = new ShortCourier();
                shortCourier.ConsignmentNumber = courier.ConsignmentNumber;
                shortCourier.Status = courier.Status;
                shortCourier.Id = courier.Id;

                shortCouriers.Add(shortCourier);
            }

            if (SearchValue != null)
            {
                if (CourierAttribute == "ConsignmentNumber")
                {
                    shortCouriers = shortCouriers.Where(courier => courier.ConsignmentNumber.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else
                {
                    shortCouriers = shortCouriers.Where(courier => courier.Status.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
            }

            CurrentPage = Math.Max(1, CurrentPage);
            totalCourierList = shortCouriers.Count();
            CurrentPage = Math.Min((totalCourierList + 9) / 10, CurrentPage);
            int takeBranch = Math.Min(totalCourierList - (CurrentPage - 1) * 10, 10);
            int skipBranch = (CurrentPage - 1) * 10;
            var paggedCouriers = shortCouriers.Skip(skipBranch).Take(takeBranch);

            return Ok(paggedCouriers);
        }

        [HttpPost("{controller}/PostCourier")]
        public async Task<IActionResult> PostCourier(int? id)
        {
            if (id != null)
            {
                var courier = _context.Courier.FirstOrDefault(x => x.Id == id);
                if (courier != null)
                {
                    _context.Courier.Remove(courier);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { success = true });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var courier = _context.Courier.FirstOrDefault(x => x.Id == id);
                if (courier != null)
                {
                    Id = courier.Id;
                    return View(courier);
                }
            }

            return NotFound();
        }

        [HttpGet("{controller}/GetBranch")]
        public IActionResult Branch()
        {
            var branch = _context.Branch.ToList();
            return Ok(branch);
        }

        [HttpGet("{controller}/GetBothBranch")]
        public IActionResult BothBranch()
        {
            if (Id != null)
            {
                var courier = _context.Courier.FirstOrDefault(x => x.Id == Id);
                if (courier != null)
                    return Ok(new { success = true, senderBranch = courier.SenderBranch, receiverBranch = courier.ReceiverBranch, status = courier.Status });

            }
            return Ok(new { success = true });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Details([Bind("Id,UserId,ConsignmentNumber,DeliveryTime,SenderName,SenderEmail,SenderPhone,SenderBranch,SenderAddress,ReceiverName,ReceiverEmail,ReceiverPhone,ReceiverBranch,ReceiverAddress,ProductQuantity,ProductPrice,Status")] Courier Courier)
        {
            if (ModelState.IsValid)
            {
                Courier.ProductPrice = (20 + Courier.ProductQuantity * 10).ToString();

                _context.Courier.Update(Courier);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "CourierList");
            }

            return View(Courier);
        }
    }
}
