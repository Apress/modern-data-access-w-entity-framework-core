using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFCExtensions;

namespace BO
{

 [Serializable]
 public class Flight 
 {

  /// <summary>
  /// Parameterless constructor
  /// </summary>
  public Flight()
  {
   // Default Values
   this.Departure = "(not set)";
   this.Destination = "(not set)";
   this.Price = 123.45m;
   this.Date = DateTime.Now;
   // Possible, but not nessesary
   //this.PassengerSet = new List<Passenger>();
   //this.BookingSet = new List<Booking>();
  }

  #region Key
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)] // No identity column!
  public int FlightNo { get; set; }
  #endregion

  #region Primitive Properties
  [StringLength(50), MinLength(3)]
  public string Departure { get; set; }
  [StringLength(50), MinLength(3)]
  public string Destination { get; set; }

  [Column("FlightDate", Order = 1 )] // TypeName = "datetime2"
  public DateTime Date { get; set; }
  public bool? NonSmokingFlight { get; set; }

  [Required]
 //[ConcurrencyCheck]
  public short? Seats { get; set; }

  //[ConcurrencyCheck]
  public short? FreeSeats { get; set; }

  //[ConcurrencyCheck]
  public decimal? Price { get; set; }

  //[ConcurrencyNoCheck]
  public string Memo { get; set; }

  /// <summary>
  /// Mapping possible, even if getters and setters are private
  /// Must be configured in context
  /// Other syntax: public int Strikebound { get; private set; }
  /// </summary>
  public bool? Strikebound
  {
   get
   {
    return _strikebound;
   }
   private set
   {
    this._strikebound = value;
   }
  }
  private bool? _strikebound;
  #endregion

  #region Calculated Columns 
  public decimal? Utilization { get; private set; }   // (see Fluent API)

  [Timestamp] // for demo: [NotMapped]
  public byte[] Timestamp { get; set; }
  #endregion

  #region Related Objects
  public Airline Airline { get; set; } 
  public ICollection<Booking> BookingSet { get; set; }
  public Pilot Pilot { get; set; }
  public Pilot Copilot { get; set; }
  [ForeignKey("AircraftTypeID")]
  public AircraftType AircraftType { get; set; }

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