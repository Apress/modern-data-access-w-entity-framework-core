using System;
using System.Collections.Generic;

namespace EFC_WWWingsV1_Reverse
{
    public partial class Flight
    {
        public Flight()
        {
            FlightPassenger = new HashSet<FlightPassenger>();
        }

        public int FlightNo { get; set; }
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime FlightDate { get; set; }
        public bool NonSmokingFlight { get; set; }
        public short Seats { get; set; }
        public short? FreeSeats { get; set; }
        public int? PilotPersonId { get; set; }
        public string Memo { get; set; }
        public bool? Strikebound { get; set; }
        public int? Utilization { get; set; }
        public byte[] Timestamp { get; set; }

        public Pilot PilotPerson { get; set; }
        public ICollection<FlightPassenger> FlightPassenger { get; set; }
    }
}
