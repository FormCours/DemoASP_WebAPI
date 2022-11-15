using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WebAPI_01.BLL.Services
{
    public class SingletonService
    {
        private static int _Count = 0;

        public int Count
        {
            get { return _Count; }
        }

        public SingletonService()
        {
            Console.WriteLine("New instance : SingletonService");
            _Count++;
        }
    }
}
