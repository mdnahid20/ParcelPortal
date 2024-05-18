using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;
using ParcelPortal.Models;
using System.Buffers;

namespace ParcelPortal.Controllers
{
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;

        public readonly ParcelPortalContext _context;
        private static string SearchValue { get; set; }

        public BranchController(ILogger<BranchController> logger, ParcelPortalContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/GetSearchValue")]
        public IActionResult SearchBranch()
        {
            return Ok(new { value = SearchValue });
        }

        [HttpPost("{controller}/PostSearchValue")]
        public IActionResult SearchBranch(string value)
        {
            SearchValue = value;
            return Ok(new { Success = true });
        }

        [HttpPost("{controller}/PostBranch")]
        public async Task<IActionResult> PostBranch(string value)
        {
            if (value != null && char.IsLetter(value[0]))
            {
                Branch branch = new Branch();
                branch.Name = value;
                _context.Branch.Add(branch);
                await _context.SaveChangesAsync();
            }
            return Ok(new { Success = true });
        }

        [HttpGet("{controller}/GetBranch")]
        public IActionResult GetBranchList()
        {
            var branch = _context.Branch.ToList();

            if (SearchValue != null)
            {
                branch = branch.Where(movie => movie.Name.ToLower().Contains(SearchValue.ToLower())).ToList();
            }

            return Ok(branch);
        }

        [HttpPost("{controller}/DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(int? id)
        {
            if (id != null)
            {
                var branch = _context.Branch.FirstOrDefault(branch => branch.Id == id);
                if (branch != null)
                {
                    _context.Branch.Remove(branch);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { Success = true });
        }
    }
}
