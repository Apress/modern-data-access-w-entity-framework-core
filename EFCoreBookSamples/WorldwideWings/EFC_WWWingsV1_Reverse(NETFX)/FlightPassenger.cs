using System;
using System.Collections.Generic;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class FlightPassenger
    {
        public int FlightFlightNo { get; set; }
        public int PassengerPersonId { get; set; }

        public Flight FlightFlightNoNavigation { get; set; }
        public Passenger PassengerPerson { get; set; }
    }
}
