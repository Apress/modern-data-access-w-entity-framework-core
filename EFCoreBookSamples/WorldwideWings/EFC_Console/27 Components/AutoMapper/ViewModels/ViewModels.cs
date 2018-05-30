using System;
using System.Collections.Generic;

namespace EFC_Console.ViewModels
{

 public class FlightView
 {
  public int FlightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public string Date { get; set; }
  public bool NonSmokingFlight { get; set; }
  public short Seats { get; set; }
  public Nullable<short> FreeSeats { get; set; }
  public Nullable<int> FlightUtilization { get; set; }

  public bool? BookedUp { get; set; }
  public string SmokerInfo { get; set; }

  public string Memo { get; set; }
  public Nullable<bool> Strikebound { get; set; }
  public byte[] Timestamp { get; set; }

  public string PilotSurname { get; set; }
  public string AircraftTypeDetailLength { get; set; }

  public override string ToString()
  {
   return "Flight " + this.FlightNo + " (" + this.Date + "): " + this.Departure + "->" + this.Destination + " Utilization: " + this.FlightUtilization + "% booked: " + this.BookedUp;
  }

  public string PilotInfo { get; set; }
  public string Copilot { get; set; }

  /// <summary>
  /// Pilot 1:1
  /// </summary>
  public PilotView Pilot { get; set; }

  /// <summary>
  /// Passengers 1:n
  /// </summary>
  public List<PassengerView> Passengers { get; set; }
 }

 public class PilotView
 {
  public int PersonId { get; set; }
  public string Surname { get; set; }
  public int Birthday { get; set; }
 }

 public class PassengerView
 {
  public PassengerView()
  {

   this.FlightViewSet = new HashSet<FlightView>();
  }

  public int PersonID { get; set; }
  public Nullable<System.DateTime> CustomerSince { get; set; }
  public int Birthday { get; set; }
  public string GivenName { get; set; }
  public string Surname { get; set; }
  public virtual ICollection<FlightView> FlightViewSet { get; set; }
 }
}
