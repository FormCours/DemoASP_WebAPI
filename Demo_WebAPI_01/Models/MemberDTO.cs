using System.ComponentModel.DataAnnotations;

namespace Demo_WebAPI_01.Models
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Email { get; set; }
    }

    public class MemberLoginDTO
    {
        [Required]
        public string Pseudo { get; set; }

        [Required]
        public string Pwd { get; set; }
    }

    public class MemberRegisterDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Pseudo { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200, MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\\W).+$")]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }
    }
}
