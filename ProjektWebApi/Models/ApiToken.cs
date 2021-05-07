using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektWebApi.Models
{
    public class ApiToken
    {
        public int Id { get; set; }
        public MyUser User { get; set; }
        public string Value { get; set; }
    }
}
