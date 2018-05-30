using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class AircraftType
    {
        public AircraftType()
        {
            Flight = new HashSet<Flight>();
        }

        public byte TypeId { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }

        public AircraftTypeDetail AircraftTypeDetail { get; set; }
        public ICollection<Flight> Flight { get; set; }
    }
}
