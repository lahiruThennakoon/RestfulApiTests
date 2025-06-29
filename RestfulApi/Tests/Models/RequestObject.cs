using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApi.Tests.Models
{
    public class Data
    {
        public String color { get; set; }
        public String size { get; set; }
    }
    public class RequestObject
    {
        public String id { get; set; }
        public String name { get; set; }
        public Data data { get; set; }
    }
}
