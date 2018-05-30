namespace BO
{
 /// <summary>
 /// Join class for join table
 /// </summary>
 public class Booking
 {
  // Composite Key: [Key] not possible, see Fluent API!
  public int FlightNo { get; set; }
  // Composite Key: [Key] not possible, see Fluent API!
  public int PassengerID { get; set; }

  public Flight Flight { get; set; }
  public Passenger Passenger { get; set; }
 }
}
