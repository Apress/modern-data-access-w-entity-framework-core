using System;
using System.Collections.Generic;

namespace BO
{

 [Serializable]
 public class Flight 
 {

  #region Key
  public int FlightNo { get; set; }
  #endregion

  #region Primitive Properties
  public string Departure { get; set; }
  public string Destination { get; set; }
  public DateTime Date { get; set; }
  public bool? NonSmokingFlight { get; set; }
  public short? Seats { get; set; }
  public short? FreeSeats { get; set; }
  public decimal? Price { get; set; }
  public string Memo { get; set; }
  #endregion

  #region Related Objects
  public ICollection<Booking> BookingSet { get; set; }
  public Pilot Pilot { get; set; }
  public Pilot Copilot { get; set; }

  // Explicit foreign key properties for the navigation properties
  public string AirlineCode { get; set; } // mandatory!
  public int PilotId { get; set; } // mandatory!
  public int? CopilotId { get; set; } // optional
  public byte? AircraftTypeID { get; set; } // optional
  #endregion

  public override string ToString()
  {
   return String.Format($"Flight #{this.FlightNo}: from {this.Departure} to {this.Destination} on {this.Date:dd.MM.yy HH:mm}: {this.FreeSeats} free Seats.");
  }

  public string ToShortString()
  {
   return String.Format($"Flight #{this.FlightNo}: {this.Departure}->{this.Destination} {this.Date:dd.MM.yy HH:mm}: {this.FreeSeats} free Seats.");
  }
 }
}