using System;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektWebApi.Models
{
    public class GeoMessage2:GeoMessage
    {
        public new MessageOne MessageOne { get; set; }
    }
}
