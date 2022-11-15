using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Services
{
    public class ScopedService
    {
        private static int _Count = 0;

        public int Count
        {
            get { return _Count; }
        }

        public ScopedService()
        {
            Console.WriteLine("New instance : ScopedService");
            _Count++;
        }
    }
}
