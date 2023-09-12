using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Controllers
{
    public class CommunityController : Controller
    {
        private readonly MovieDbContext _context;

        public CommunityController(MovieDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdminCommunity()
        {
            var com = _context.Community.ToList();
            return View(com);
        }

        public IActionResult AdminCreateCommunity()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminCreateCommunity(Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Community.Add(community);
                _context.SaveChanges();
                return RedirectToAction("AdminCommunity");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // 오류 메시지 로깅
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(community);
        }

        public async Task<IActionResult> AdminCommunityDelete(int? id)
        {
            if (id == null || _context.Community == null)
            {
                return NotFound();
            }
            var stdData = await _context.Community.FirstOrDefaultAsync((x => x.Id == id));

            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var stdData = await _context.Community.FindAsync(id);
            if (stdData != null)
            {
                _context.Community.Remove(stdData);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("AdminCommunity");
        }

        [HttpGet]
        public IActionResult CommunityUser()
        {
            var com = _context.Community.ToList();
            return View(com);
        }
    }
}
