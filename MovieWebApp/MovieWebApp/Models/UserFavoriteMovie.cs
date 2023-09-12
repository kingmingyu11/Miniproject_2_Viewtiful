using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Models
{
    public class UserFavoriteMovie
    {
        [Key] // 기본 키로 사용될 속성을 지정합니다.
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string MovieTitle { get; set; }
        public Movie Movie { get; set; }
    }
}
