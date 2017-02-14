using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBControl
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new ControlRepository();
            var result = repo.GetMatches();
        }
    }
}
