
using Demo_WebAPI_01.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Interfaces
{
    public interface IMemberService
    {
        Member? Login(string pseudo, string pwd);

        Member? Register(Member memberData);
    }
}
