using System;

namespace BO
{
 public class Employee : Person
 {
  public DateTime? HireDate;
  public float Salary { get; set; }
  public virtual Employee Supervisor { get; set; }

  public string PassportNumber => this._passportNumber;
  private string _passportNumber;

  public void SetPassportNumber(string passportNumber)
  {
   this._passportNumber = passportNumber;
  }
 }
}
