using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Email { get; set; }
        public string HashPwd { get; set; }
        public string Role { get; set; }
    }
}
