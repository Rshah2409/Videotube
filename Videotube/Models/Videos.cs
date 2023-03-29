using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videotube.Models
{
    public class Videos
    {
        public string Video_ID { get; set; }
        public int User_ID { get; set; }
        public string First_name { get; set;}
        public string Last_name { get; set; }
        public string Email_ID { get; set; }
        public string Password { get; set; }
        public string ID_Proof { get; set; }
        public string Status { get; set; }
        public string Plan_name1 { get; set;}
        public string V_category { get; set; }
      //  public string Sub_ID { get; set; }
        public string V_title { get; set; }
        public string Sub_Duration { get; set; }
        public string V_file { get; set; }
        public string up_date { get; set; }
        public string Date { get; set; }
        public string Exp_Date { get; set; }
        public string Payment_status { get; set; }
        public string V_description { get; set; }
        public string Expert_ID { get; set; }
        public int Sub_ID { get; set; }
        public int Payment_amt { get; set; }
        public string Plan_name { get; set; }

        public string Sub_charge { get; set; }
        public List<Videos> videos { get; set; }
    }
}