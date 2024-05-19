using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;
using ParcelPortal.ViewModels;

namespace ParcelPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourierListController : Controller
    {
        public readonly ParcelPortalContext _context;
        private static string SearchValue { get; set; }
        private static string CourierAttribute { get; set; } = "ConsignmentNumber";
        public CourierListController( ParcelPortalContext context)
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
        public IActionResult SearchCourier(string option ,string value)
        {
            CourierAttribute = option;
            SearchValue = value;

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

            return Ok(shortCouriers);
        }

        [HttpPost("{controller}/PostCourier")]
        public async Task<IActionResult> PostCourier(int? id)
        {
            if(id != null)
            {
                var courier = _context.Courier.FirstOrDefault(x => x.Id == id);
                if(courier != null)
                {
                    _context.Courier.Remove(courier);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { success = true });
        }


    }
}
