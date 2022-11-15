using System.ComponentModel.DataAnnotations;

namespace Demo_WebAPI_01.Models
{
    public class PersonnageDTO
    {
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class PersonnageDataDTO
    {
        [Required]
        public string? Firstname { get; set; }
        
        [Required]
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
