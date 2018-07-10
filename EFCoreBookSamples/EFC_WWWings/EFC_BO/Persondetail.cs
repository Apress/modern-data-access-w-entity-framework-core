using System.ComponentModel.DataAnnotations;

namespace BO
{
 public class Persondetail
 {

  [Key]
  public int ID { get; set; }
  public string Memo { get; set; }
  public byte[] Photo { get; set; }
  [MaxLength(30)]
  public string Street { get; set; }

  [MaxLength(30)]
  public string City { get; set; }
  [MaxLength(3)]
  public string Country { get; set; }
  [MaxLength(8)]
  public string Postcode { get; set; }
  // for schema migration demo: v2 of schema
  [MaxLength(130)]
  public string Planet
  {
   get;
   set;
  }

  // optional for schema migration demo: v3 of schema
  //[MaxLength(130)]
  //public string Quadrant
  //{
  // get;
  // set;
  //}
 }
}
