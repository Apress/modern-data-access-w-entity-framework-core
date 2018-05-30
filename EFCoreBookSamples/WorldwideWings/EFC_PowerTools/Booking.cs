using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Booking
    {
        public int FlightNo { get; set; }
        public int PassengerId { get; set; }

        public Flight FlightNoNavigation { get; set; }
        public Passenger Passenger { get; set; }
    }
}
