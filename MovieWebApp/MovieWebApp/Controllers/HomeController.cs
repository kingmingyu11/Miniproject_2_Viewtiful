using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;
using System.Diagnostics;
using System.Net;

namespace MovieWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieDbContext _context;

        public HomeController(MovieDbContext context)
        {
            _context = context;
        }
        public IActionResult Start()
        {
            return View();
        }
        public IActionResult BeforeLogin()
        {
            return View();
        }
        public IActionResult AdminGenre()
        {
			var genres = new List<string> { "로맨스", "범죄/스릴러", "SF/판타지", "코미디", "드라마", "액션" }; // 예제 장르 목록

			// 각 장르에 대한 영화 목록을 가져옵니다.
			var moviesByGenre = new Dictionary<string, List<Movie>>();
			foreach (var genre in genres)
			{
				var movies = _context.Movie.Where(m => m.Genre == genre).ToList();
				moviesByGenre.Add(genre, movies);
			}

			ViewData["Genres"] = genres; // 뷰에 장르 목록을 전달합니다.
			return View(moviesByGenre);
		}
        public IActionResult Index(User user)
        {
            // 초기 렌더링 시 오류 메시지를 초기화합니다.
            ModelState.Clear();

            var dbUser = _context.User.FirstOrDefault(u => u.Address == user.Address && u.Pw == user.Pw);

            if (dbUser != null && user.Address == "admin")
            {
                // 관리자 로그인 성공
                ViewBag.UserRole = "Admin"; // ViewBag에 사용자 역할 저장
                return RedirectToAction("Admin");
            }
            else if (dbUser != null && user.Address != "admin")
            {
                // 사용자 로그인 성공
                ViewBag.UserRole = "User"; // ViewBag에 사용자 역할 저장
                return RedirectToAction("Home");
            }
            else
            {
                // 로그인 실패
                return View("Index");
            }
        }
        public IActionResult User()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User([Bind("Id,Pw,Name,Address,Gender,Day,Hp")] User users)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(users);
        }
        public IActionResult Admin()
        {
            var topMovies = _context.Movie.OrderByDescending(m => m.Views).Take(10).ToList(); // 상위 10개 영화 가져오기
            ViewBag.Layout = "Admin_Layout";
            return View(topMovies);
        }
        public IActionResult AdminSearch(string searchTerm)
        {
            var viewModel = new MovieReviewViewModel();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // 검색어가 없는 경우, 모든 영화를 가져와 설정합니다.
                viewModel.Movies = _context.Movie.ToList();
            }
            else
            {
                // 검색어와 일치하는 영화를 가져옵니다.
                var matchedMovies = _context.Movie
                    .Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();

                viewModel.Movies = matchedMovies;
            }

            return View(viewModel);
        }
        public IActionResult AdminReviewboard(string searchTerm)
        {
            List<Review> reviews;

            if (string.IsNullOrEmpty(searchTerm))
            {
                reviews = _context.Review.ToList();
            }
            else
            {
                reviews = _context.Review
                    .Where(r => r.Title.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
            }

            return View(reviews);
        }
        public IActionResult Home()
		{
			// 모든 장르 목록을 가져옵니다. (데이터베이스에서 가져오거나 하드코딩할 수 있음)
			var genres = new List<string> { "로맨스", "범죄/스릴러", "SF/판타지", "코미디", "드라마", "액션" }; // 예제 장르 목록

			// 각 장르에 대한 영화 목록을 가져옵니다.
			var moviesByGenre = new Dictionary<string, List<Movie>>();
			foreach (var genre in genres)
			{
				var movies = _context.Movie.Where(m => m.Genre == genre).ToList();
				moviesByGenre.Add(genre, movies);
			}
            ViewBag.Layout = "User_Layout";
            ViewData["Genres"] = genres; // 뷰에 장르 목록을 전달합니다.
			return View(moviesByGenre);
		}
        public IActionResult Genre()
        {
            // 모든 장르 목록을 가져옵니다. (데이터베이스에서 가져오거나 하드코딩할 수 있음)
            var genres = new List<string> { "로맨스", "범죄/스릴러", "SF/판타지", "코미디", "드라마", "액션" }; // 예제 장르 목록

            // 각 장르에 대한 영화 목록을 가져옵니다.
            var moviesByGenre = new Dictionary<string, List<Movie>>();
            foreach (var genre in genres)
            {
                var movies = _context.Movie.Where(m => m.Genre == genre).ToList();
                moviesByGenre.Add(genre, movies);
            }

            ViewData["Genres"] = genres; // 뷰에 장르 목록을 전달합니다.
            return View(moviesByGenre);
        }
        public IActionResult Detail(string title)
        {
            // 제목을 기반으로 영화 정보를 데이터베이스에서 조회
            var movie = _context.Movie.FirstOrDefault(m => m.Title == title.Replace("_", " ")); // Replace 메서드를 사용해 공백을 다시 복원합니다.

            if (movie == null)
            {
                return NotFound(); // 영화를 찾을 수 없는 경우 404 에러를 반환합니다.
            }

            var firstReviewWithMatchingTitle = _context.Review.FirstOrDefault(r => r.Title == movie.Title);

            var viewModel = new MovieReviewViewModel
            {
                Movie = movie,
                Reviews = new List<Review> { firstReviewWithMatchingTitle } // 첫 번째 리뷰를 리스트에 담아서 할당합니다.
            };

            return View(viewModel);
        }

        public IActionResult TopMovie()
		{
			// Views를 기준으로 내림차순으로 정렬하고 상위 12개 영화를 가져옵니다.
			var topMovies = _context.Movie.OrderByDescending(m => m.Views).Take(12).ToList();

			return View(topMovies);
		}
		public IActionResult Chart()
		{
			// 상위 12개 영화를 가져와서 데이터 전달
			var topMovies = _context.Movie.OrderByDescending(m => m.Views).Take(12).ToList();
			return View(topMovies);
		}
		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile Video, IFormFile Picture)
        {
            //if (ModelState.IsValid)
            //{
                // 영상(mp4)과 사진(jpg)을 바이너리 데이터로 변환하여 모델에 저장
                if (Video != null && Video.Length > 0)
                {
                    using (var videoStream = new MemoryStream())
                    {
                        Video.CopyTo(videoStream);
                        movie.Video = videoStream.ToArray();
                    }
                }

                if (Picture != null && Picture.Length > 0)
                {
                    using (var pictureStream = new MemoryStream())
                    {
                        Picture.CopyTo(pictureStream);
                        movie.Picture = pictureStream.ToArray();
                    }
                }

                // 데이터베이스에 저장
                _context.Movie.Add(movie);
                _context.SaveChanges();

                return RedirectToAction("Home"); // 등록 후 리다이렉트
            //}

            //return View(movie); // 유효성 검사 오류 시 폼 다시 표시
        }
        //리뷰조회
        public IActionResult Reviewboard(string searchTerm)
        {
            List<Review> reviews;

            if (string.IsNullOrEmpty(searchTerm))
            {
                reviews = _context.Review.ToList();
            }
            else
            {
                reviews = _context.Review
                    .Where(r => r.Title.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
            }

            return View(reviews);
        }
        //리뷰등록
        public IActionResult Reviewenroll()
        {
            return View();
        }
        //리뷰를 둥록할때 비동기로 처리해서 DB에 저장.
        [HttpPost]
        public async Task<IActionResult> Reviewenroll(Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Review.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Reviewboard");
            }
            return View(review);
        }
        //리뷰 수정 코드 냥
        public async Task<IActionResult> ReviewEdit(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var reviewData = await _context.Review.FindAsync(id);

            if (reviewData == null)
            {
                return NotFound();
            }
            return View(reviewData);
        }
        //리뷰 수정 비동식으로 ReviewDB에 넣는 코드 냥
        [HttpPost]
        public async Task<IActionResult> ReviewEdit(int? id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //_context.Student.Update(std);
                _context.Update(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Reviewboard", "Home");
            }
            return View(review);
        }
        //리뷰 조회임돠
        public async Task<IActionResult> ReviewDetails(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var reviewData = await _context.Review.FirstOrDefaultAsync(x => x.Id == id);

            if (reviewData == null)
            {
                return NotFound();
            }

            return View(reviewData);
        }
        //등록된 리뷰 삭제하는 코드
        public async Task<IActionResult> ReviewDelete(int? id)
        {

            if (id == null || _context.Review == null)
            {
                return NotFound();
            }
            var reviewData = await _context.Review.FirstOrDefaultAsync((x => x.Id == id));

            if (reviewData == null)
            {
                return NotFound();
            }
            return View(reviewData);
        }
        [HttpPost, ActionName("ReviewDelete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var reviewData = await _context.Review.FindAsync(id);
            if (reviewData != null)
            {
                _context.Review.Remove(reviewData);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ReviewBoard", "Home");
        }


        [HttpPost]
        public IActionResult AddFavorite(string title)
        {
            // 사용자 ID를 가져오는 로직을 추가해야 합니다.
            // 예를 들어, 현재 사용자의 ID를 세션에서 가져오는 방법은 다음과 같습니다.
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // 사용자가 로그인하지 않았으면 로그인 페이지로 리다이렉트 또는 오류 처리
                return RedirectToAction("Login");
            }

            // 사용자가 이미 해당 영화를 찜했는지 확인
            var isAlreadyFavorite = _context.UserFavoriteMovie
                .Any(ufm => ufm.UserId == userId && ufm.MovieTitle == title);

            if (isAlreadyFavorite)
            {
                // 이미 찜한 경우 - 찜 취소
                var favoriteMovie = _context.UserFavoriteMovie
                    .SingleOrDefault(ufm => ufm.UserId == userId && ufm.MovieTitle == title);
                if (favoriteMovie != null)
                {
                    _context.UserFavoriteMovie.Remove(favoriteMovie);
                    _context.SaveChanges();
                    return Json(new { success = true, isFavorite = false });
                }
            }
            else
            {
                // 찜하지 않은 경우 - 찜하기
                var userFavoriteMovie = new UserFavoriteMovie
                {
                    UserId = userId.Value,
                    MovieTitle = title
                };
                _context.UserFavoriteMovie.Add(userFavoriteMovie);
                _context.SaveChanges();
                return Json(new { success = true, isFavorite = true });
            }

            // 사용자의 찜 여부에 따라 응답을 반환
            return Json(new { success = false, isFavorite = false });
        }
        //검색
        public IActionResult Search(string searchTerm)
        {
            var viewModel = new MovieReviewViewModel();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // 검색어가 없는 경우, 모든 영화를 가져와 설정합니다.
                viewModel.Movies = _context.Movie.ToList();
            }
            else
            {
                // 검색어와 일치하는 영화를 가져옵니다.
                var matchedMovies = _context.Movie
                    .Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();

                viewModel.Movies = matchedMovies;
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}