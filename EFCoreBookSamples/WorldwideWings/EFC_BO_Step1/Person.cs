using System;

namespace BO
{
 [Serializable]
 public class Person
 {
  #region Primary Key
  public int PersonID { get; set; }
  #endregion

  #region Primitive properties
  public string Surname { get; set; }
  public string GivenName { get; set; }
  public Nullable<DateTime> Birthday { get; set; }
  public virtual string EMail { get; set; }
  #endregion

  // Calculated property (in RAM only)
  public string FullName => this.GivenName + " " + this.Surname;

  public override string ToString()
  {
   return "#" + this.PersonID + ": " + this.FullName;
  }
 }
}
