using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;

namespace ParcelPortal.Controllers
{
    [Authorize]
    public class TrackProductController : Controller
    {
        public readonly ParcelPortalContext _context;
        public TrackProductController(ParcelPortalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("{controller}/ConsigmentNumber")]
        public async Task<IActionResult> ConsigmentNumber(string value)
        {
            var courier = _context.Courier.FirstOrDefault(x => x.ConsignmentNumber == value);

            if (courier == null)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success=true, courier = courier });
        }
    }
}
