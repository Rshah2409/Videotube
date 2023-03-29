using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videotube.Models
{
    public class Plan_list
    {
     
        public int Sub_ID { get; set; }

        public string Plan_name { get; set; }
        public string Sub_charge { get; set; }
        public string Sub_Duration { get; set; }

        public string up_date { get; set; }
        public string Plan_name1 { get; set; }
        public List<Plan_list> Planlist { get; set; }

    }
}