using System;
using System.Collections.Generic;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class Pilot
    {
        public Pilot()
        {
            Flight = new HashSet<Flight>();
        }

        public int PersonId { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string Flightscheintyp { get; set; }
        public int? FlightHours { get; set; }
        public string FlightSchool { get; set; }

        public Employee Person { get; set; }
        public ICollection<Flight> Flight { get; set; }
    }
}
