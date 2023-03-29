 
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Videotube.Models;
using System.Net.Mail;
using Videotube.Common;

namespace Videotube.Controllers
{
    public class ViewerController : Controller
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ritik\OneDrive\Desktop\Videotube\Videotube\App_Data\Database1.mdf;Integrated Security=True");

        // GET: Viewer
        public ActionResult Home()
        {
            con.Open();
            //Table data of video_upload
            SqlCommand cmd = new SqlCommand("select up_date,V_category,Plan_name1,V_title,V_file,Video_ID,Expert_ID from Video_upload  ", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            //Show table data to particular page
            Videos vd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Videos vd = new Videos();
                vd.Video_ID = ds.Tables[0].Rows[i]["Video_ID"].ToString();
                vd.V_category = ds.Tables[0].Rows[i]["V_category"].ToString();
                vd.V_title = ds.Tables[0].Rows[i]["up_date"].ToString();
                vd.V_title = ds.Tables[0].Rows[i]["V_title"].ToString();
                vd.Plan_name1 = ds.Tables[0].Rows[i]["Plan_name1"].ToString();
                vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();
                vd.Expert_ID = ds.Tables[0].Rows[i]["Expert_ID"].ToString();

                videos.Add(vd);
            }
            vd1.videos = videos;
            con.Close();

         

           

            return View(vd1);

           
        }

        public ActionResult Registration()
        {
            return View();
        }
       

        [HttpPost]
        public ActionResult Registration(HttpPostedFileBase ID_Proof, FormCollection Data)
        {

            //Password EncryptData = new Password();

            //ID  file 
            String img1 = System.IO.Path.GetFileName(ID_Proof.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Content/ID_viewer"), img1);
            ////ID file is uploaded

            ID_Proof.SaveAs(path);


            string imagelink = "~/Content/ID_viewer/"+ img1;

           
            String First_name = Data["First_name"];
            String Last_name = Data["Last_name"];
            String Email_ID = Data["Email_ID"];
            String Password = Data["Password"];
            String Repeat_Password = Data["Repeat_Password"];
            String ID_proof = imagelink;

            con.Open();
            if (Password==Repeat_Password)
            {
                SqlCommand cmd = new SqlCommand("Insert Into Viwere_reg Values(@First_name,@Last_name,@Email_ID,@Password,@ID_Proof,@Status)", con);
                cmd.Parameters.AddWithValue("@First_name", First_name);
                cmd.Parameters.AddWithValue("@Last_name", Last_name);
                cmd.Parameters.AddWithValue("@Email_ID", Email_ID);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@ID_Proof", ID_proof);
                cmd.Parameters.AddWithValue("@Status", 0);
                cmd.ExecuteNonQuery();
                con.Close();

                ViewData["msg"]= " You are registered successfuly!!!!";
            }
            else
            {
                ViewData["msg"]= " Password Not Matched!!!!!";
            }
            return View();
        }

        public ActionResult watch_alert()
        {
            TempData["msg"] = "First Login!";
            return RedirectToAction ("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection Data)
        {
            String Email_ID = Data["Email_ID"];
            String Password = Data["Password"];
            //Password EncryptData = new Password();
            //string pass = EncryptData.Encode(Password);


            con.Open();

            SqlCommand cmd = new SqlCommand("select * from Viwere_reg where Email_ID='" + Email_ID + "' and password='" + Password + "'", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                string name = ds.Tables[0].Rows[0]["First_name"].ToString();
               

                Session["fname"] = name;
                Session["Viwere_ID"] = Email_ID;


                ViewData["msgl"] = " Welcome To Eduction world!!!!!";
                





        }



            else
            {

                ViewData["msgl"] = " Password Not Matched!!!!!";

            }
            return View();

        }

        public ActionResult Forget()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Fp(string email)
        {


            con.Open();
            SqlCommand cmd = new SqlCommand("select First_name,Last_name,Password,Email_ID from Viwere_reg where Email_ID='" + email + "' ", con);
            cmd.ExecuteScalar();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {

                string K = ds.Tables[0].Rows[0]["First_name"].ToString();
                string K1 = ds.Tables[0].Rows[0]["Last_name"].ToString();
                string K2 = ds.Tables[0].Rows[0]["Password"].ToString();
                con.Close();

                MailMessage mm = new MailMessage("videostube1718@gmail.com", email);
            mm.Subject = "Invoice";
             mm.Body = "Dear Mr./Mrs." + K1 + "<hr /> <br/> Your password is: " + K2 + ".<br/><br/>VideosTube.<br/> <br/> Thank You";

            mm.Body = "TEST EMAIL FROM US!!!!!";
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "videostube1718@gmail.com";
            NetworkCred.Password = "HRPS1718";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
           // TempData["Message3"] = "Password send to Email";
            return RedirectToAction("Login");

            }
            else
            {
                TempData["Message1"] = "Invalid Email";
                return RedirectToAction("Fp");

            }


            
        }

        public ActionResult Videos()
        {


            con.Open();
            //Table data of video_upload
            SqlCommand cmd = new SqlCommand("select V_file from Video_upload  ", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            //Show table data to particular page
            Videos vd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Videos vd = new Videos();

                vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();





                videos.Add(vd);
            }
            vd1.videos = videos;
            con.Close();

            return View(vd1);

        }


        //    [HttpPost]
        //    public ActionResult Videos(FormCollection Data)
        //    {

        //        con.Open();
        //        //Table data of video_upload
        //        SqlCommand cmd = new SqlCommand("select V_file from Video_upload where  Expert_ID='" +Session["Email_ID"].ToString() + " ' ", con);
        //        cmd.ExecuteScalar();

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);


        //        //Show table data to particular page
        //        Videos vd1 = new Videos();
        //        List<Videos> videos = new List<Videos>();
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            Videos vd = new Videos();

        //            vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();





        //            videos.Add(vd);
        //        }
        //        vd1.videos = videos;
        //        con.Close();

        //        return View(vd1);

        //    }

        //        return View();
        //    }




        public ActionResult Feedback()

        {


            return View();
        }

        [HttpPost]


        public ActionResult Feedback(FormCollection Data)

        {

            String Name = Data["Name"];
            String Email_ID = Data["Email_ID"];
            String Subject = Data["Subject"];
            String Comment = Data["Comment"];

            con.Open();

            SqlCommand cmd = new SqlCommand("Insert Into Viewer_Feedback Values(@Name,@Email_ID,@Subject,@Comment)", con);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Email_ID", Email_ID);
            cmd.Parameters.AddWithValue("@Subject ", Subject);
            cmd.Parameters.AddWithValue("@Comment ", Comment);
            cmd.ExecuteNonQuery();
            con.Close();

            ViewData["msgfeed"]= " Successfully Given";
            return View();


        }



        public ActionResult Contain()
        {

            string vid = Request.QueryString["vid"].ToString();

            ///Select
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select * from Details where u_email='" + @Session["Viwere_ID"] + "' and Video_ID='" + vid + "' ", con);

            cmd1.ExecuteScalar();

            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            con.Close();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //string name = ds.Tables[0].Rows[0]["First_name"].ToString();
                //string status = ds.Tables[0].Rows[0]["Status"].ToString();

                ///Alert
                TempData["subscribe"] = "You have already subscribed video!";
                return RedirectToAction("Home");
            }
            else
            {
                //Table data of video_upload
                SqlCommand cmd = new SqlCommand("select a.Plan_name1,a.up_date,a.Expert_ID,a.Video_ID,a.V_category,a.V_title,a.V_file,a.V_description,b.Sub_ID,b.Plan_name,b.Sub_Duration,b.Sub_charge from Video_upload as a, Subscription_plan as b where a.Video_ID='" + vid + "' and a.Plan_name1=b.Sub_ID", con);
                con.Open();
                cmd.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);


                //Show table data to particular page
                Videos vd1 = new Videos();
                List<Videos> videos = new List<Videos>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Videos vd = new Videos();
                    vd.V_category = ds.Tables[0].Rows[i]["V_category"].ToString();
                    vd.up_date = ds.Tables[0].Rows[i]["up_date"].ToString();
                    vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();
                    vd.V_description = ds.Tables[0].Rows[i]["V_description"].ToString();
                    vd.V_title = ds.Tables[0].Rows[i]["V_title"].ToString();
                    vd.Video_ID = ds.Tables[0].Rows[i]["Video_ID"].ToString();
                    vd.Plan_name = ds.Tables[0].Rows[i]["Plan_name"].ToString();
                    vd.Sub_Duration = ds.Tables[0].Rows[i]["Sub_Duration"].ToString();
                    vd.Plan_name1 = ds.Tables[0].Rows[i]["Plan_name1"].ToString();
                    vd.Expert_ID = ds.Tables[0].Rows[i]["Expert_ID"].ToString();
                    vd.Sub_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Sub_ID"].ToString());
                    vd.Sub_charge = ds.Tables[0].Rows[i]["Sub_charge"].ToString();
                    videos.Add(vd);
                }

                vd1.videos = videos;

                con.Close();

                return View(vd1);
            }
            //return View();
        }

        public ActionResult Watch_video()

        {
            if (Session["Viwere_ID"]==null)
            {
                ViewData["Watch"]= " If you want to watch the video please login first!!!";

                //return RedirectToAction("Home");

            }
            else
            {


            }



            
            return View();
        }
        public ActionResult Details()
        {
            
            return View();
        }

        public ActionResult Library()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select a.Sub_ID,a.Plan_Name,a.Exp_Date,b.V_title,b.V_file,b.V_description,b.V_category from Details as a, Video_upload as b where a.u_email='" + @Session["Viwere_ID"] + " ' ", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Videos vd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Videos vd = new Videos();
                vd.Sub_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Sub_ID"].ToString());
                vd.Plan_name = ds.Tables[0].Rows[i]["Plan_name"].ToString();
                vd.Exp_Date = ds.Tables[0].Rows[i]["Exp_Date"].ToString();
                vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();
                vd.V_title = ds.Tables[0].Rows[i]["V_title"].ToString();
                vd.V_description = ds.Tables[0].Rows[i]["V_description"].ToString();
                vd.V_category = ds.Tables[0].Rows[i]["V_category"].ToString();

                videos.Add(vd);
            }



            vd1.videos = videos;

            con.Close();

            return View(vd1);

            
        }
      
        public ActionResult Save_vid()
        {
            string Sub_ID = Request.QueryString["Sub_ID"].ToString();
            string Video_ID = Request.QueryString["vid"].ToString();
            string Plan_Name = Request.QueryString["plan_name"].ToString();
            string duration = Request.QueryString["dur"].ToString();
            string amount = Request.QueryString["total"].ToString();
            int dd = Convert.ToInt32(duration);



            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            DateTime newDate = dt.AddDays(Convert.ToDouble(dd));
            string Exp_Date = newDate.ToString("MM/dd/yyyy");

            




            con.Open();

            SqlCommand cmd = new SqlCommand("Insert Into Details Values(@Sub_ID,@Video_ID,@Plan_Name,@Date,@Exp_Date,@Payment_amt,@u_email)", con);
            cmd.Parameters.AddWithValue("@Sub_ID", Sub_ID);
            cmd.Parameters.AddWithValue("@Video_ID", Video_ID);
            cmd.Parameters.AddWithValue("@Plan_Name ", Plan_Name);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("MM/dd/yyyy"));
            cmd.Parameters.AddWithValue("@Exp_Date", Exp_Date);
            cmd.Parameters.AddWithValue("@Payment_amt", amount);
            cmd.Parameters.AddWithValue("@u_email", Session["Viwere_ID"].ToString());
            cmd.ExecuteNonQuery();
            con.Close();

            TempData["itemdate"] = DateTime.Now.ToString("MM/dd/yyyy");
            TempData["expdate"] = Exp_Date;
            TempData["planname"] = Plan_Name;
            TempData["amt"] = amount;

          

            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Home");
        }

        



    }
}