using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPortal.Data;
using ParcelPortal.Models;
using ParcelPortal.ViewModels;
using System.Buffers;

namespace ParcelPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;

        public readonly ParcelPortalContext _context;
        private static int CurrentPage { get; set; } = 1;
        private static int totalBranch { get; set; }
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
        public async Task<IActionResult> AddBranch(string value)
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

        [HttpGet("{controller}/GetPageNumber")]
        public IActionResult PageNumber() 
        {
            int lastPage = (totalBranch + 9) / 10;
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

        [HttpGet("{controller}/GetBranch")]
        public IActionResult GetBranchList()
        {
            var branch = _context.Branch.ToList();

            if (SearchValue != null)
            {
                branch = branch.Where(x => x.Name.ToLower().Contains(SearchValue.ToLower())).ToList();
            }

            CurrentPage = Math.Max(1, CurrentPage);
            totalBranch = branch.Count();
            CurrentPage = Math.Min((totalBranch + 9) / 10, CurrentPage);
            int takeBranch = Math.Min(totalBranch - (CurrentPage - 1) * 10, 10);
            int skipBranch = (CurrentPage - 1) * 10;
            var paggedBranchs = branch.Skip(skipBranch).Take(takeBranch);

            return Ok(paggedBranchs);
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
