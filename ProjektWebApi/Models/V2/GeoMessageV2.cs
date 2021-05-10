﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektWebApi.Models.V2
{
    public class GeoMessageV2
    {
        public int Id { get; set; }
        public Message Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
