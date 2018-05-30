using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BO
{
 public class PassengerStatus
 {
  public const string A = "A";
  public const string B = "B";
  public const string C = "C";
  public static string[] PassengerStatusSet = { PassengerStatus.A, PassengerStatus.B, PassengerStatus.C };
 }

 [Serializable]
 public partial class Passenger : Person
 {
  
  public Passenger()
  {
   this.BookingSet = new List<Booking>();
  }


  // Primary key is inherited!
  #region Primitive Properties
  public virtual Nullable<DateTime> CustomerSince { get; set; }

  [StringLength(1), MinLength(1), RegularExpression("[ABC]")]
  public virtual string Status { get; set; }
  #endregion

  #region Relations
  public virtual ICollection<Booking> BookingSet { get; set; }
  #endregion
 }
}
