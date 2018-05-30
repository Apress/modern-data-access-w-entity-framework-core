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
  // optional for schema migration demo:
  // [MaxLength(30)]
  //public virtual string Planet
  //{
  // get;
  // set;
  //}
 }
}
