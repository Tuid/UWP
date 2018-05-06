using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class PM
    {
        public class Result
        {
            public string city { get; set; }
            public string PM2 { get; set; }
            public string AQI { get; set; }
            public string quality { get; set; }
            public string PM10 { get; set; }
            public string CO { get; set; }
            public string NO2 { get; set; }
            public string O3 { get; set; }
            public string SO2 { get; set; }
            public string time { get; set; }

            public class RootObject
            {
                public string resultcode { get; set; }
                public string reason { get; set; }
                public List<Result> result { get; set; }
                public string error_code { get; set; }
            }
        }

    }
}