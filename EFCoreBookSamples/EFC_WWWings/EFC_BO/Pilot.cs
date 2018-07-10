using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BO
{

 public enum PilotLicenseType
 {
  // https://en.wikipedia.org/wiki/Pilot_licensing_and_certification
  Student, Sport, Recreational, Private, Commercial, FlightInstructor, ATP
 }


 public enum PilotLicenseType2
 {
  // https://en.wikipedia.org/wiki/Pilot_licensing_and_certification
  Student, Sport, Recreational, Private, Commercial, FlightInstructor, ATP
 }



 [Serializable]
 public partial class Pilot : Employee
 {
  // PK is inherited from Employee
  
  #region Primitive Properties
  public virtual DateTime LicenseDate { get; set; }
  public virtual Nullable<int> FlightHours { get; set; }

  public virtual PilotLicenseType? PilotLicenseType
  {
   get;
   set;
  }

  //public virtual PilotLicenseType2 PilotLicenseType2
  //{
  // get;
  // set;
  //}

  [StringLength(50)]
  public virtual string FlightSchool
  {
   get;
   set;
  }
  #endregion

  #region Related Objects
  public virtual ICollection<Flight> FlightAsPilotSet { get; set; }
  public virtual ICollection<Flight> FlightAsCopilotSet { get; set; }
  #endregion
 }
}
