using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Flight
    {
        public Flight()
        {
            Booking = new HashSet<Booking>();
        }

        public int FlightNo { get; set; }
        public byte? AircraftTypeId { get; set; }
        public string AirlineCode { get; set; }
        public int? CopilotId { get; set; }
        public DateTime FlightDate { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public short? FreeSeats { get; set; }
        public DateTime LastChange { get; set; }
        public string Memo { get; set; }
        public bool? NonSmokingFlight { get; set; }
        public int PilotId { get; set; }
        public decimal? Price { get; set; }
        public short Seats { get; set; }
        public bool? Strikebound { get; set; }
        public decimal? Utilization { get; set; }
        public byte[] Timestamp { get; set; }

        public AircraftType AircraftType { get; set; }
        public Airline AirlineCodeNavigation { get; set; }
        public Employee Copilot { get; set; }
        public Employee Pilot { get; set; }
        public ICollection<Booking> Booking { get; set; }
    }
}
