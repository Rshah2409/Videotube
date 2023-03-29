using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videotube.Models
{
    public class view_video
    {

       
        public int Sub_ID { get; set; }
        public string Plan_name1 { get; set; }
        public string Video_ID { get; set; }
        public string V_category { get; set; }
        public string V_title { get; set; }
        public string V_file { get; set; }
        public string V_description { get; set; }
        public string up_date { get; set; }
        public string Expert_ID { get; set; }
        public string Plan_name { get; set; }
        public List<view_video> viewvideo { get; set; }
    
}
}