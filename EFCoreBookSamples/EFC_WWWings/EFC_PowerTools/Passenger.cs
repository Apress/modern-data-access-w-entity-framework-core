using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class Passenger
    {
        public Passenger()
        {
            Booking = new HashSet<Booking>();
        }

        public int PersonId { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CustomerSince { get; set; }
        public int? DetailId { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string Status { get; set; }
        public string Surname { get; set; }

        public Persondetail Detail { get; set; }
        public ICollection<Booking> Booking { get; set; }
    }
}
