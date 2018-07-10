using System;
using System.Collections.Generic;

namespace EFC_PowerTools
{
    public partial class AircraftTypeDetail
    {
        public byte AircraftTypeId { get; set; }
        public float? Length { get; set; }
        public string Memo { get; set; }
        public short? Tare { get; set; }
        public byte? TurbineCount { get; set; }

        public AircraftType AircraftType { get; set; }
    }
}
