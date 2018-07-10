using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Airline
    {
        public Airline()
        {
            Flight = new HashSet<Flight>();
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<Flight> Flight { get; set; }
    }
}
