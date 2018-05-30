using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Employee
    {
        public Employee()
        {
            FlightCopilot = new HashSet<Flight>();
            FlightPilot = new HashSet<Flight>();
            InverseSupervisorPerson = new HashSet<Employee>();
        }

        public int PersonId { get; set; }
        public DateTime? Birthday { get; set; }
        public int? DetailId { get; set; }
        public string Discriminator { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string PassportNumber { get; set; }
        public float Salary { get; set; }
        public int? SupervisorPersonId { get; set; }
        public string Surname { get; set; }
        public int? FlightHours { get; set; }
        public string FlightSchool { get; set; }
        public DateTime? LicenseDate { get; set; }
        public int? PilotLicenseType { get; set; }

        public Persondetail Detail { get; set; }
        public Employee SupervisorPerson { get; set; }
        public ICollection<Flight> FlightCopilot { get; set; }
        public ICollection<Flight> FlightPilot { get; set; }
        public ICollection<Employee> InverseSupervisorPerson { get; set; }
    }
}
