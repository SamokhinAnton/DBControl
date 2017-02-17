using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBControl
{
    public class ScoresDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }

        public string Name { get; set; }

        public int Minute { get; set; }
    }
}
