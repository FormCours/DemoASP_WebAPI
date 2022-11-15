namespace Demo_WebAPI_01.BLL.Models
{
    public class Personnage
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber { get; set; }

        public Personnage(int id, string firstname, string lastname, DateTime? birthdate, string? phoneNumber)
        {
            Firstname = firstname;
            Lastname = lastname;
            Birthdate = birthdate;
            PhoneNumber = phoneNumber;
            Id = id;
        }
    }
}
