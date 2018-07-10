using System;

namespace BO
{
 [Serializable]
 public abstract class Person
 {
  #region Primitive properties
  // --- Primary Key by convention classnameID
  public int PersonID { get; set; }
  // --- Additional properties
  public string Surname { get; set; }
  public string GivenName { get; set; }
  public Nullable<DateTime> Birthday { get; set; }
  public virtual string EMail { get; set; }
  #endregion

  #region Relations
  public virtual  Persondetail Detail { get; set; } = new Persondetail(); // mandatory (no FK property!)
  #endregion

  // Calculated property (in RAM only)
  public string FullName => this.GivenName + " " + this.Surname;

  public override string ToString()
  {
   return "#" + this.PersonID + ": " + this.FullName;
  }
 }
}
