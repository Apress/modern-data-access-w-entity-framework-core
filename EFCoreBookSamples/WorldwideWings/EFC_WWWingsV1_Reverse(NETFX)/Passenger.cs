using System;
using System.Collections.Generic;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class Passenger
    {
        public Passenger()
        {
            FlightPassenger = new HashSet<FlightPassenger>();
        }

        public int PersonId { get; set; }
        public DateTime? CustomerSince { get; set; }
        public string PassengerStatus { get; set; }

        public Person Person { get; set; }
        public ICollection<FlightPassenger> FlightPassenger { get; set; }
    }
}
