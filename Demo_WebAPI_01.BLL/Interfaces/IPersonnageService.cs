using Demo_WebAPI_01.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Interfaces
{
    public interface IPersonnageService
    {
        Personnage? GetById(int id);

        IEnumerable<Personnage> Get();

        Personnage Insert(Personnage personnage);

        bool Update(int id, Personnage personnage);

        bool Delete(int id);
    }
}
