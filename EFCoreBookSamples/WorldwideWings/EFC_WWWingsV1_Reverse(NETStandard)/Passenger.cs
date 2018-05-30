using System;
using System.Collections.Generic;

namespace EFC_WWWingsV1_Reverse
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
        public HashSet<FlightPassenger> FlightPassenger { get; set; }
    }
}
