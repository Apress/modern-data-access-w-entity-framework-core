using System;
using System.Collections.Generic;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class Employee
    {
        public Employee()
        {
            InverseSupervisorPerson = new HashSet<Employee>();
        }

        public int PersonId { get; set; }
        public int? EmployeeNo { get; set; }
        public DateTime? HireDate { get; set; }
        public int? SupervisorPersonId { get; set; }

        public Person Person { get; set; }
        public Employee SupervisorPerson { get; set; }
        public Pilot Pilot { get; set; }
        public ICollection<Employee> InverseSupervisorPerson { get; set; }
    }
}
