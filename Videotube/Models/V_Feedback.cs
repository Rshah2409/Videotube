using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videotube.Models
{
    public class V_Feedback
    {
        public int Viewer_ID { get; set; }

        public string Name { get; set; }
        public string Email_ID { get; set; }
        public int  Exp_ID { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }

      

        public List<V_Feedback> VFeedback { get; set; }

    }
}