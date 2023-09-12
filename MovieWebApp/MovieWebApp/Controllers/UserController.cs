using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;
using System.Diagnostics;

namespace MovieWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly MovieDbContext _context;

        public UserController(MovieDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdminUser()
        {
            var user = await _context.User.ToListAsync<User>();
            return View(user);
        }

        //----회원정보 생성-----//


        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction("User");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // 오류 메시지 로깅
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(user);
        }

        //디테일


        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var stdData = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

            if (stdData == null)
            {
                return NotFound();
            }

            return View(stdData);
        }

        // 수정
        public async Task<IActionResult> UserEdit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var stdData = await _context.User.FindAsync(id);

            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(int? id, User std)
        {
            if (id != std.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //_context.Student.Update(std);
                _context.Update(std);
                await _context.SaveChangesAsync();
                return RedirectToAction("User");

            }
            return View(std);

        }


        //회원정보 삭제
        public async Task<IActionResult> AdminUserDelete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }
            var stdData = await _context.User.FirstOrDefaultAsync((x => x.Id == id));

            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var stdData = await _context.User.FindAsync(id);
            if (stdData != null)
            {
                _context.User.Remove(stdData);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("AdminUser");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}