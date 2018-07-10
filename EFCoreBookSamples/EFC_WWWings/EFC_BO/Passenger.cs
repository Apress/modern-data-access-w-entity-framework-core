using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BO
{
 public static class PocoLoadingExtensions
 {
  public static TRelated Load<TRelated>(
      this Action<object, string> loader,
      object entity,
      ref TRelated navigationField,
      [CallerMemberName] string navigationName = null)
      where TRelated : class
  {
   loader?.Invoke(entity, navigationName);

   return navigationField;
  }
 }

 public class PassengerStatus
 {
  public const char A = 'A';
  public const char B = 'B';
  public const char C = 'C';
  public static char[] PassengerStatusSet = { PassengerStatus.A, PassengerStatus.B, PassengerStatus.C };
 }

 [Serializable]
 public partial class Passenger : Person
 {
  //private Action<object, string> LazyLoader { get; set; }
  public Passenger()
  {
   this.BookingSet = new List<Booking>();
  }

  /// <summary>
  /// Injection for Lazy Loading without Runtime Proxies
  /// </summary>
  //private Passenger(Action<object, string> lazyLoader)
  //{
  // LazyLoader = lazyLoader;
  //}

 
  // Primary key is inherited!
  #region Primitive Properties
  public virtual Nullable<DateTime> CustomerSince { get; set; }

  public string FrequentFlyer { get; set; }

  [StringLength(1), MinLength(1), RegularExpression("[ABC]")]
  public virtual char Status { get; set; }
  #endregion

  #region Relations
  public virtual ICollection<Booking> BookingSet { get; set; }

  //private ICollection<Booking> _BookingSet;
  //public virtual ICollection<Booking> BookingSet
  //{
  // // Lazy Loading without Runtime Proxies
  // get => LazyLoader?.Load(this, ref _BookingSet);
  // set => _BookingSet = value;
  //}
  #endregion
 }
}
