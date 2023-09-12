namespace MovieWebApp.Models
{
    public class MovieReviewViewModel
    {
        public Movie Movie { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Movie> Movies { get; set; }
        public byte[] MovieImage { get; set; } // 영화 이미지를 나타내는 속성
    }
}
