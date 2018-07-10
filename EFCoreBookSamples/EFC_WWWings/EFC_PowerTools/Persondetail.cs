using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Persondetail
    {
        public Persondetail()
        {
            Employee = new HashSet<Employee>();
            Passenger = new HashSet<Passenger>();
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Memo { get; set; }
        public byte[] Photo { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }

        public ICollection<Employee> Employee { get; set; }
        public ICollection<Passenger> Passenger { get; set; }
    }
}
