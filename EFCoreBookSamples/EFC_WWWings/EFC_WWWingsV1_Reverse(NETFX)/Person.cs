using System;
using System.Collections.Generic;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Country { get; set; }
        public DateTime? Birthday { get; set; }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Memo { get; set; }

        public Employee Employee { get; set; }
        public Passenger Passenger { get; set; }
    }
}
