namespace EFC_Console.AutoMapper
{
 public class FlightDTOShort
 {
  public int FlightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
 }

 public class FlightDTO
 {
  public int FlightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public string Date { get; set; }
 }
}