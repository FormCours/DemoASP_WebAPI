using Demo_WebAPI_01.BLL.Models;

namespace Demo_WebAPI_01.Models.Mappers
{
    public static class PersonnageMapper
    {
        public static PersonnageDTO toDTO(this Personnage data)
        {
            return new PersonnageDTO()
            {
                Id = data.Id,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Birthdate = data.Birthdate,
                PhoneNumber = data.PhoneNumber
            };
        }

        public static Personnage toModel(this PersonnageDTO dataDTO)
        {
            if (string.IsNullOrWhiteSpace(dataDTO.Firstname) || string.IsNullOrWhiteSpace(dataDTO.Lastname))
            {
                throw new ArgumentNullException();
            }

            return new Personnage(dataDTO.Id ?? -1, dataDTO.Firstname, dataDTO.Lastname, dataDTO.Birthdate, dataDTO.PhoneNumber);
        }

        public static Personnage toModel(this PersonnageDataDTO dataDTO)
        {
            if(string.IsNullOrWhiteSpace(dataDTO.Firstname) || string.IsNullOrWhiteSpace(dataDTO.Lastname))
            {
                throw new ArgumentNullException();
            }

            return new Personnage(-1, dataDTO.Firstname, dataDTO.Lastname, dataDTO.Birthdate, dataDTO.PhoneNumber);
        }
    }
}
