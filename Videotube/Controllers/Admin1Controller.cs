using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videotube.Models;

namespace Videotube.Controllers
{
    public class Admin1Controller : Controller
    {
        
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ritik\OneDrive\Desktop\Videotube\Videotube\App_Data\Database1.mdf;Integrated Security=True");
        // GET: Admin1
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection Data)
        {
            //string Email_Add = Data["Email_Add"];
            string User_name = Data["UName"];
            string Password = Data["Pass"];
            Session["User_name"] = User_name;
            if (Password=="Admin1" && User_name=="Admin1")
            {
                ViewData["msg"] = "Welcome!!";
            }
            else
            {
                ViewData["msg"] = "Something went wrong!!";
            }

            return View();
        }
        public ActionResult Home()
        {

            return View();
        }
        public ActionResult Forgot()
        {

            return View();
        }
        [HttpPost]
        public ActionResult FPS(string Email)
        {


            //con.Open();
            //SqlCommand cmd = new SqlCommand("select First_name,Last_name,Password,Email_ID from reg_exp where Email_ID='" + Email + "' ", con);
            //cmd.ExecuteScalar();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    string K = ds.Tables[0].Rows[0]["First_name"].ToString();
            //    string K1 = ds.Tables[0].Rows[0]["Last_name"].ToString();
            //    string K2 = ds.Tables[0].Rows[0]["Password"].ToString();
            //    con.Close();

            //    MailMessage mm = new MailMessage("videostube1718@gmail.com", Email);
            //    mm.Subject = "Invoice";
            //    mm.Body = "Dear Mr./Mrs." + K1 + "<hr /> <br/> Your password is: " + K2 + ".<br/><br/>VideosTube.<br/> <br/> Thank You";

            //    mm.Body = "TEST EMAIL FROM US!!!!!";
            //    mm.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.EnableSsl = true;
            //    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //    NetworkCred.UserName = "videostube1718@gmail.com";
            //    NetworkCred.Password = "HRPS1718";
            //    smtp.UseDefaultCredentials = true;
            //    smtp.Credentials = NetworkCred;
            //    smtp.Port = 587;
            //    smtp.Send(mm);
            //    //TempData["Message3"] = "Password send to Email";
            //    return RedirectToAction("Login");

            //}
            //else
            //{
            //    TempData["Message1"] = "Invalid Email";
            //    return RedirectToAction("fp");

            //}
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("/Admin1/Login");
           
        }

        public ActionResult Expert_Au()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from reg_exp", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Videos vdd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Videos vddd1 = new Videos();
                vddd1.User_ID = Convert.ToInt16(ds.Tables[0].Rows[i]["User_ID"].ToString());
                vddd1.First_name = ds.Tables[0].Rows[i]["First_name"].ToString();
                vddd1.Last_name = ds.Tables[0].Rows[i]["Last_name"].ToString();
                vddd1.Email_ID = ds.Tables[0].Rows[i]["Email_ID"].ToString();
                vddd1.ID_Proof = ds.Tables[0].Rows[i]["ID_Proof"].ToString();
                vddd1.Status = ds.Tables[0].Rows[i]["Status"].ToString();
               
                videos.Add(vddd1);




            }

            vdd1.videos = videos;

            con.Close();



            return View(vdd1);



        }
          
        public ActionResult Expert_Update()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Expert_Update(FormCollection Data)
        {
            //string First_name = Data["fname"];
            //string Last_name = Data["Lname"];
            string Email_ID = Data["email"];
            //string ID_Proof = Data["id"];
            string status = Data["status"];

           

            SqlCommand cmd = new SqlCommand("UPDATE reg_exp SET Status='"+status+"' where Email_ID='"+Email_ID+"'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            TempData["msg"] = "Successfully Updated";
            return RedirectToAction("Expert_Au");

            


        }
        public ActionResult Learner_Au()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Viwere_reg", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Videos vdd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Videos vddd1 = new Videos();
                vddd1.User_ID = Convert.ToInt16(ds.Tables[0].Rows[i]["User_ID"].ToString());
                vddd1.First_name = ds.Tables[0].Rows[i]["First_name"].ToString();
                vddd1.Last_name = ds.Tables[0].Rows[i]["Last_name"].ToString();
                vddd1.Email_ID = ds.Tables[0].Rows[i]["Email_ID"].ToString();
                vddd1.ID_Proof = ds.Tables[0].Rows[i]["ID_Proof"].ToString();
                vddd1.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                videos.Add(vddd1);




            }

            vdd1.videos = videos;

            con.Close();



            return View(vdd1);



        }
        public ActionResult Video_manage()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select a.Video_ID,a.V_category,a.V_description,a.V_title,a.V_file,a.Expert_ID,b.Status,b.Email_ID,b.First_name,b.Last_name from Video_upload as a, reg_exp as b where a.Expert_ID=b.Email_ID", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Videos vdd1 = new Videos();
            List<Videos> videos = new List<Videos>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Videos vddd1 = new Videos();
                vddd1.Video_ID = (ds.Tables[0].Rows[i]["Video_ID"].ToString());
                vddd1.V_category = ds.Tables[0].Rows[i]["V_category"].ToString();
                vddd1.V_description = ds.Tables[0].Rows[i]["V_description"].ToString();
                vddd1.V_title = ds.Tables[0].Rows[i]["V_title"].ToString();
                vddd1.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();
                vddd1.Status = ds.Tables[0].Rows[i]["status"].ToString();
                vddd1.Email_ID = ds.Tables[0].Rows[i]["Email_ID"].ToString();
                vddd1.First_name = ds.Tables[0].Rows[i]["First_name"].ToString();
                vddd1.Last_name = ds.Tables[0].Rows[i]["Last_name"].ToString();
                videos.Add(vddd1);

            }

            vdd1.videos = videos;

            con.Close();



            return View(vdd1);



        }


        public ActionResult Dashboard()
        {
            con.Open();
            //no of experts
            SqlCommand cmd = new SqlCommand("SELECT Count (Email_ID) from reg_exp", con);
            object CAN = cmd.ExecuteScalar();
            ViewData["M"] = CAN.ToString();

            //no of learners
            SqlCommand cmd1 = new SqlCommand("SELECT Count (Email_ID) from Viwere_reg", con);
            object CAN1 = cmd1.ExecuteScalar();
            ViewData["M1"] = CAN1.ToString();

            //no of videos
            SqlCommand cmd2 = new SqlCommand("SELECT Count (Video_ID) from Video_upload", con);
            object CAN2 = cmd2.ExecuteScalar();
            ViewData["M2"] = CAN2.ToString();

            //feedback from viwewr
            SqlCommand cmd3 = new SqlCommand("SELECT Count (Viewer_ID) from Viewer_Feedback", con);
            object CAN3 = cmd3.ExecuteScalar();
            ViewData["M3"] = CAN3.ToString();

            //feedback from expert
            SqlCommand cmd4 = new SqlCommand("SELECT Count (Exp_ID) from exp_feedback", con);
            object CAN4 = cmd4.ExecuteScalar();
            ViewData["M4"] = CAN4.ToString();

            return View();
        }
        public ActionResult Learner_Update()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Learner_Update(FormCollection Data)
        {
            //string First_name = Data["fname"];
            //string Last_name = Data["Lname"];
            string Email_ID = Data["email"];
            //string ID_Proof = Data["id"];
            string status = Data["status"];



            SqlCommand cmd = new SqlCommand("UPDATE Viwere_reg SET Status='" + status + "' where Email_ID='" + Email_ID + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            TempData["msg"] = "Successfully Updated";
            return RedirectToAction("Learner_Au");




        }

        public ActionResult Feedback_Ex(FormCollection Data)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from exp_feedback ", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            V_Feedback pd1 = new V_Feedback();
            List<V_Feedback> VFeedback = new List<V_Feedback>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                V_Feedback pd = new V_Feedback();

                pd.Exp_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Exp_ID"].ToString());
                pd.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                pd.Email_ID = ds.Tables[0].Rows[i]["Email_ID"].ToString();
                pd.Subject = ds.Tables[0].Rows[i]["Subject"].ToString();
                pd.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();

                VFeedback.Add(pd);

            }

            pd1.VFeedback = VFeedback;

            con.Close();



            return View(pd1);



        }
        public ActionResult Feedback_Le(FormCollection Data)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Viewer_Feedback ", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            V_Feedback pd1 = new V_Feedback();
            List<V_Feedback> VFeedback = new List<V_Feedback>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                V_Feedback pd = new V_Feedback();

                pd.Viewer_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Viewer_ID"].ToString());
                pd.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                pd.Email_ID = ds.Tables[0].Rows[i]["Email_ID"].ToString();
                pd.Subject = ds.Tables[0].Rows[i]["Subject"].ToString();
                pd.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();

                VFeedback.Add(pd);

            }

            pd1.VFeedback = VFeedback;

            con.Close();



            return View(pd1);



        }

    }
    
    }
