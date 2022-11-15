using Demo_WebAPI_01.BLL.Interfaces;
using Demo_WebAPI_01.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Services
{
    public class PersonnageService : IPersonnageService
    {
        #region FakeDB
        private static List<Personnage> _Personnages = new List<Personnage>
        {
            new Personnage(1, "Zaza", "Vanderquack", null, null),
            new Personnage(2, "Della", "Duck", new DateTime(1991, 5, 1), "+32 123 45 67" ),
            new Personnage(3, "Balthazar", "Picsou", new DateTime(1957, 2, 16), "+32 987 65 43")
        };

        private static int _LastId = 3;
        #endregion


        private SingletonService _SingletonService;

        public PersonnageService(SingletonService singletonService)
        {
            _SingletonService = singletonService;
        }


        public Personnage? GetById(int id)
        {
            return _Personnages.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Personnage> Get()
        {
            return _Personnages.AsReadOnly();
        }

        public Personnage Insert(Personnage personnage)
        {
            // TODO Test de garde

            Personnage data = new Personnage(
                ++_LastId,
                personnage.Firstname,
                personnage.Lastname,
                personnage.Birthdate,
                personnage.PhoneNumber
            );
            _Personnages.Add(data);

            return data;
        }


        public bool Update(int id, Personnage personnage)
        {
            Personnage? target = _Personnages.SingleOrDefault(p => p.Id == id);

            if (target is null)
                return false;

            target.Firstname = personnage.Firstname;
            target.Lastname = personnage.Lastname;
            target.Birthdate = personnage.Birthdate;
            target.PhoneNumber = personnage.PhoneNumber;

            return true;
        }

        public bool Delete(int id)
        {
            int nbRow = _Personnages.RemoveAll(p => p.Id == id);
            return nbRow == 1;
        }
    }
}
