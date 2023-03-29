using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Videotube.Models;
using Videotube.Common;

namespace Videotube.Controllers
{
    public class ExpertController : Controller
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ritik\OneDrive\Desktop\Videotube\Videotube\App_Data\Database1.mdf;Integrated Security=True");
        // GET: Expert
        public ActionResult Home()
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
            String Email_ID = Data["Email_ID"];
            String Password = Data["Password"];
            //Password EncryptData = new Password();
            //string pass = EncryptData.Encode(Password);

            con.Open();

            SqlCommand cmd = new SqlCommand("select * from reg_exp where Email_ID='" + Email_ID + "' and password='" + Password + "'", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                string name = ds.Tables[0].Rows[0]["First_name"].ToString();
                string status = ds.Tables[0].Rows[0]["Status"].ToString();
                

                if(status == "0")
                {
                    ViewData["msg1"] = "Your account has been blocked by Admin!";
                }
                else
                {
                    Session["name"] = name;
                    Session["Email_ID"] = Email_ID;

                    ViewData["msg"] = " Welcome To Dashbroad!!!!!";
                }

               
                
               
            }
            else
            {
             
                ViewData["msg1"] = " Password Not Matched!!!!!";

            }
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(HttpPostedFileBase ID_Proof, FormCollection Data)
        {

            //ID  file 
            String img = System.IO.Path.GetFileName(ID_Proof.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Content/ID_IMG"), img);
            //ID file is uploaded

            ID_Proof.SaveAs(path);


            string imagelink = "~/Content/ID_IMG/" + img;

            //Password EncryptData = new Password();

            String First_name = Data["First_name"];
            String Last_name = Data["Last_name"];
            String Email_ID = Data["Email_ID"];
            String Password = Data["Password"];
            String Repeat_Password = Data["Repeat_Password"];
            String ID_proof = imagelink;

            con.Open();
            if (Password == Repeat_Password)
            {
                SqlCommand cmd = new SqlCommand("Insert Into reg_exp Values(@First_name,@Last_name,@Email_ID,@Password,@ID_Proof,@Status)", con);
                cmd.Parameters.AddWithValue("@First_name", First_name);
                cmd.Parameters.AddWithValue("@Last_name", Last_name);
                cmd.Parameters.AddWithValue("@Email_ID", Email_ID);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@ID_Proof", ID_proof);
                cmd.Parameters.AddWithValue("@Status",0);
                cmd.ExecuteNonQuery();
                con.Close();

                ViewData["msg"] = " You are registered successfuly!!!!";
            }
            else
            {
                ViewData["msg"] = " Password Not Matched!!!!!";
            }
            return View();
        }


        public ActionResult Dashboard()
        {
            con.Open();
            //no of videos
            SqlCommand cmd = new SqlCommand("SELECT Count (Expert_ID) from Video_upload where Expert_ID='" + Session["Email_ID"] + "'", con);
            object CAN = cmd.ExecuteScalar();
            ViewData["Message"]=CAN.ToString();

            
  

            //no of plans
            SqlCommand cmd2 = new SqlCommand("SELECT Count (DISTINCT Sub_ID ) from Subscription_plan where Expert_ID='" + Session["Email_ID"] + "'", con);
            object CAN2= cmd2.ExecuteScalar();
            ViewData["M2"] = CAN2.ToString();

            //earning
            int K1 = 0;
            
            SqlCommand cmd6 = new SqlCommand("select a.Video_ID,a.Payment_amt,b.Video_ID,b.Expert_ID from Details as a,Video_upload as b where b.Expert_Id='"+ Session["Email_ID"] + "'  and a.Video_ID=b.Video_ID", con);
            cmd6.ExecuteScalar();
            SqlDataAdapter da = new SqlDataAdapter(cmd6);
            DataSet ds = new DataSet();
            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

               int  K2 = Convert.ToInt32(ds.Tables[0].Rows[i]["Payment_amt"].ToString());
               K1=K1 + K2;

            }
            ViewData["M3"] = K1.ToString();

            //subscribed users
          
            SqlCommand cmd8 = new SqlCommand("select a.Video_ID,b.Video_ID,b.Expert_ID from Details as a,Video_upload as b where b.Expert_Id='" + Session["Email_ID"] + "'  and a.Video_ID=b.Video_ID", con);
            cmd8.ExecuteScalar();
            SqlDataAdapter da8 = new SqlDataAdapter(cmd8);
            DataSet ds8 = new DataSet();
            da.Fill(ds8);

           // for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
              //  string K11 = ds.Tables[0].Rows[i]["u_email"].ToString();
                            

            //}
           
            ViewData["M1"] = ds.Tables[0].Rows.Count.ToString();



            //sum of earning
            //SqlCommand cmd3 = new SqlCommand("SELECT Sum(CAST(Payment_amt as int)) from Details", con);
            //object CAN3 = cmd3.ExecuteScalar();
            //ViewData["M90"] = CAN3.ToString();
            con.Close();

            return View();
        }

        public ActionResult Upload()
        {


            SqlCommand cmd2 = new SqlCommand("select Sub_ID,Plan_name from Subscription_plan where Expert_ID='" + Session["Email_ID"].ToString() + " ' ", con);
            con.Open();
            SqlDataReader reader = cmd2.ExecuteReader();
            List<Plan_list> str = new List<Plan_list>();

            while (reader.Read())
            {
                Plan_list mycard = new Plan_list();

                mycard.Sub_ID = Convert.ToInt32(reader[0].ToString());
                mycard.Plan_name1 = reader[1].ToString();
                //  str.Add(reader.GetValue(j).ToString());
                //j++;

                str.Add(mycard);
            }
            reader.Close();
            con.Close();


            ViewBag.Plan_name = str;


            return View();
        }


        [HttpPost]

        public ActionResult Upload1(HttpPostedFileBase V_file, FormCollection Data)


        {

            //video file 
            String vid = System.IO.Path.GetFileName(V_file.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Content/Video"), vid);
            //video file is uploaded

            V_file.SaveAs(path);
            string imagelink = "~/Content/Video/" + vid;

            String ss_plan = Data["ss_plan"];
            String V_category = Data["V_category"];
            String V_title = Data["V_title"];
            String v_file = imagelink;
            String V_description = Data["V_description"];

            Response.Write(ss_plan);

            con.Open();

            SqlCommand cmd = new SqlCommand("Insert Into Video_upload Values(@Plan_name1,@V_category,@V_title,@V_file,@V_description,@up_date,@Expert_ID)", con);
            cmd.Parameters.AddWithValue("@Plan_name1", ss_plan);
            cmd.Parameters.AddWithValue("@V_category", V_category);
            cmd.Parameters.AddWithValue("@V_title", V_title);
            cmd.Parameters.AddWithValue("@V_file", v_file);
            cmd.Parameters.AddWithValue("@V_description", V_description);

            cmd.Parameters.AddWithValue("@up_date", DateTime.Now.ToString("dd/MM/yyyy"));
            cmd.Parameters.AddWithValue("@Expert_ID ", Session["Email_ID"].ToString());
            cmd.ExecuteNonQuery();
            con.Close();

            //return View();
            TempData["msg12"] = " Successfully Uploaded";

            return RedirectToAction("Upload");
        }



        public ActionResult View_video()
        {
            
            con.Open();
            //Table data of video_upload
            SqlCommand cmd = new SqlCommand("select * from Video_upload where  Expert_ID='" + Session["Email_ID"].ToString() + " ' ", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            
            //Show table data to particular page
            view_video vd1 = new view_video();
            List<view_video> viewvideo = new List<view_video>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                view_video vd = new view_video();
               
                vd.Video_ID = ds.Tables[0].Rows[i]["Video_ID"].ToString();
                vd.Plan_name1 = ds.Tables[0].Rows[i]["Plan_name1"].ToString();
                vd.V_category = ds.Tables[0].Rows[i]["V_category"].ToString();
                vd.V_title = ds.Tables[0].Rows[i]["V_title"].ToString();
                vd.V_file = ds.Tables[0].Rows[i]["V_file"].ToString();
                vd.V_description = ds.Tables[0].Rows[i]["V_description"].ToString();
                
                viewvideo.Add(vd);
            }
            
            vd1.viewvideo = viewvideo;
            con.Close();

            return View(vd1);

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("/Viewer/Home");
        }
          public ActionResult Plan_list()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Subscription_plan where Expert_ID='" + Session["Email_ID"].ToString() + " ' ", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            Plan_list pd1 = new Plan_list();
            List<Plan_list> Planlist = new List<Plan_list>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Plan_list pd = new Plan_list();

                pd.Sub_ID = Convert.ToInt16(ds.Tables[0].Rows[i]["Sub_ID"].ToString());
                pd.Plan_name = ds.Tables[0].Rows[i]["Plan_name"].ToString();
                pd.Sub_charge = ds.Tables[0].Rows[i]["Sub_charge"].ToString();
                pd.Sub_Duration = ds.Tables[0].Rows[i]["Sub_Duration"].ToString();
                pd.up_date = ds.Tables[0].Rows[i]["up_date"].ToString();

                Planlist.Add(pd);

            }

            pd1.Planlist = Planlist;

            con.Close();



            return View(pd1);
        }

        public ActionResult Create_plan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_plan(FormCollection Data)
        {
            String S_plan = Data["S_plan"];
            String S_charge = Data["S_charge"];
            String Duration = Data["Duration"];
            con.Open();
           
            
            SqlCommand cmd = new SqlCommand("Insert Into Subscription_plan Values(@Plan_name1,@Sub_charge,@Sub_Duration,@up_date,@Expert_ID)", con);
            cmd.Parameters.AddWithValue("@Plan_name1", S_plan);
            cmd.Parameters.AddWithValue("@Sub_charge", S_charge);
            cmd.Parameters.AddWithValue("@Sub_Duration ", Duration);
            cmd.Parameters.AddWithValue("@up_date", DateTime.Now.ToString("dd/MM/yyyy"));
            cmd.Parameters.AddWithValue("@Expert_ID ", Session["Email_ID"].ToString());
            cmd.ExecuteNonQuery();
            con.Close();
            ViewData["msg"] = " Successfully Created";
            return View();
        }

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

            SqlCommand cmd = new SqlCommand("Insert Into exp_feedback Values(@Name,@Email_ID,@Subject,@Comment)", con);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Email_ID", Email_ID);
            cmd.Parameters.AddWithValue("@Subject ", Subject);
            cmd.Parameters.AddWithValue("@Comment ", Comment);
            cmd.ExecuteNonQuery();
            con.Close();

            ViewData["msgfeed"] = " Successfully Given";
            return View();


        }


        public ActionResult View_feedback(FormCollection Data)

        {
            String Email_ID = Data["Email_ID"];
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Viewer_Feedback where Email_ID='" + Email_ID + " ' ", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            V_Feedback pd1 = new V_Feedback();
            List<V_Feedback> VFeedback = new List<V_Feedback>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                V_Feedback pd = new V_Feedback();

                pd.Viewer_ID = Convert.ToInt16(ds.Tables[0].Rows[i]["Viewer_ID"].ToString());
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



        public ActionResult Plan_Update()

        {


            return View();
        }
        [HttpPost]

        public ActionResult Plan_Update(FormCollection Data)

        {
            String Sub_ID = Data["pid"];
            String S_plan = Data["Su_plan"];
            String S_charge = Data["Su_charge"];
            String Duration = Data["S_Duration"];
            string up_date = Data["up_date"];
            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE Subscription_plan  SET Plan_name='" + S_plan + "' ,Sub_charge='" + S_charge + "',Sub_Duration='" + Duration + "',up_date='" + up_date + "'   WHERE Sub_ID= '" + Sub_ID + " ' ", con);

            cmd.ExecuteNonQuery();
            con.Close();

            //return View();
            TempData["msg"] = " Successfully Updated";

            return RedirectToAction("Plan_Update");
        }




        public ActionResult View_update()

        {


            SqlCommand cmd2 = new SqlCommand("select Sub_ID,Plan_name1 from subscription_plan where Expert_ID='" + Session["exemail"].ToString() + "'  ", con);
            con.Open();
            SqlDataReader reader = cmd2.ExecuteReader();
            List<Plan_list> str = new List<Plan_list>();

            while (reader.Read())
            {
                Plan_list mycard = new Plan_list();

                mycard.Sub_ID = Convert.ToInt32(reader[0].ToString());
                mycard.Plan_name1 = reader[1].ToString();
                //  str.Add(reader.GetValue(j).ToString());
                //j++;

                str.Add(mycard);
            }
            reader.Close();
            con.Close();


            ViewBag.Plan_name = str;


            return View();
        }

        [HttpPost]

        public ActionResult View_update(HttpPostedFileBase V_file, FormCollection Data)

        {
            //video file 
            String vid = System.IO.Path.GetFileName(V_file.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Content/Video"), vid);
            //video file is uploaded

            V_file.SaveAs(path);
            string imagelink = "~/Content/Video/" + vid;

            String Video_id = Data["vid"];
            String pl_name = Data["p_name"];
            String vi_category = Data["v_category"];
            String vi_title = Data["v_title"];
            String vi_description = Data["v_description"];
            String vi_file = imagelink;
            String up_date = Data["update"];


            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE Video_upload SET Plan_name1='" + pl_name + "' ,V_category='" + vi_category + "',V_title='" + vi_title + "' ,V_file='" + vi_file + "'   ,V_description='" + vi_description + "',up_date='" + up_date + "'   WHERE Video_ID= '" + Video_id + " ' ", con);

            cmd.ExecuteNonQuery();
            con.Close();


            //return View();
            TempData["msg"] = " Successfully Updated";

            return RedirectToAction("View_update");
        }

        public ActionResult Forget()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Fpp(string Email)
        {


            con.Open();
            SqlCommand cmd = new SqlCommand("select First_name,Last_name,Password,Email_ID from reg_exp where Email_ID='" + Email + "' ", con);
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

                MailMessage mm = new MailMessage("videostube1718@gmail.com", Email);
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
                //TempData["Message3"] = "Password send to Email";
                return RedirectToAction("Login");

            }
            else
            {
                TempData["Message1"] = "Invalid Email";
                return RedirectToAction("Fpp");

            }
        }
           
            public ActionResult Update_Password()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Update_Password(FormCollection Data)
        {

            String Ol_pass = Data["O_pass"];
            String Ne_pass = Data["N_pass"];

            //Response.Write(Ol_pass);
            //Response.Write(Ne_pass);



            con.Open();

            SqlCommand cmd = new SqlCommand("select Password from reg_exp  ", con);
            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();

            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                string pass = ds.Tables[0].Rows[k]["Password"].ToString();
                //Response.Write(pass);

                //string pass = Data["Password"];
                if (pass == Ol_pass)
                {
                    con.Open();

                    SqlCommand cm = new SqlCommand("UPDATE reg_exp  SET Password='" + Ne_pass + "' where Email_ID='" + Session["Email_ID"] + " '", con);
                    cm.Parameters.AddWithValue("@Password", Ne_pass);

                    cm.ExecuteNonQuery();
                    con.Close();



                    ViewData["msg"] = " Password is Updated!!!!!";
                }

                else
                {
                    ViewData["msg"] = " Password Not Matched!!!!!";
                }
            }
            return View();

        }
        public ActionResult Details(FormCollection Data)
        {
            //int Date = Convert.ToInt32(Request.QueryString["Date"].ToString());
           
           // DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
           // DateTime newDate = dt.AddDays(Convert.ToDouble(Date));
           // string Exp_Date = newDate.ToString("dd/MM/yyyy");
            //string Sub_ID1 = Convert.ToInt32(Data["Sub_ID"]).ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand("select a.Sub_ID, a.Video_ID, a.Plan_name, a.Date, a.Exp_Date, b.Video_ID, b.Expert_ID from  Details as a,Video_upload as b where b.Expert_ID='"+Session["Email_ID"]+"' and a.Video_ID=b.Video_ID", con);

            cmd.ExecuteScalar();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


                Videos vdd1 = new Videos();
                List<Videos> videos = new List<Videos>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    Videos vddd1 = new Videos();

                vddd1.Sub_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Sub_ID"].ToString());
                vddd1.Video_ID = ds.Tables[0].Rows[i]["Video_ID"].ToString();
                vddd1.Plan_name = ds.Tables[0].Rows[i]["Plan_name"].ToString();
                vddd1.Date = ds.Tables[0].Rows[i]["Date"].ToString();
                vddd1.Exp_Date = ds.Tables[0].Rows[i]["Exp_Date"].ToString();
                vddd1.Payment_amt = Convert.ToInt32(ds.Tables[0].Rows[i]["Payment_amt"].ToString());
                videos.Add(vddd1);




            }

            vdd1.videos = videos;

            con.Close();



            return View(vdd1);



        }
            
        
        
       public ActionResult Update_profile()
        {


            return View();
        }
        [HttpPost]


        public ActionResult Update_profile(FormCollection Data)
        {


            return View();
        }


    }
}