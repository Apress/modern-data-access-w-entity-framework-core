using System;
using System.Collections.Generic;

namespace EFC_WWWingsV1_Reverse
{
    public partial class Protokoll
    {
        public int Id { get; set; }
        public DateTime Zeit { get; set; }
        public string Computer { get; set; }
        public string Benutzer { get; set; }
        public string Text { get; set; }
        public string Entity { get; set; }
        public string Attribut { get; set; }
        public string Aktion { get; set; }
        public int? EntityId { get; set; }
        public string AlterWert { get; set; }
        public string NeuerWert { get; set; }
    }
}
