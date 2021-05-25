using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Farmasi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sertifikalar()
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        public ActionResult NedenUyeOlmaliyim()
        {
            return View();
        }

        public ActionResult KayitOl()
        {
            return View();
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult Katalog()
        {
            return View();
        }

        public string GetData(string data)
        {
            try
            {
                var json = JObject.Parse(data);

                var name = json["name"].ToString();

                var phone = json["phone"].ToString();

                var tc = json["tc"].ToString();

                var il = json["il"].ToString();

                var ilce = json["ilce"].ToString();

                var adress = json["adress"].ToString();

                var dogumTarih = json["dogumTarih"].ToString();

                var isTc = TCKontrol(tc);

                if (isTc == 1) return "0";

                var isInsert = InsertForm(name, phone, tc, il, ilce, adress, dogumTarih);

                if (isInsert == 0) return "0";

                PostMail("gulnarozpak@hotmail.com", name, phone, tc, il, ilce, adress, dogumTarih);

                PostMail("sunaydurak54@gmail.com", name, phone, tc, il, ilce, adress, dogumTarih);

                PostMail("info@farmasi-uyekaydi.com", name, phone, tc, il, ilce, adress, dogumTarih);

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public int TCKontrol(string tc)
        {
            using (SqlConnection openCon = new SqlConnection("Data Source=94.138.197.30;Initial Catalog=farmasiuyelik;User ID=farmasiuyelik;Password=Sananeamk54;"))
            {
                string saveStaff = $"SELECT * FROM Customers WHERE tc = {tc}";

                using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                {
                    querySaveStaff.Connection = openCon;
                    openCon.Open();
                    using (SqlDataReader reader = querySaveStaff.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return 1;
                        }
                    }
                    openCon.Close();
                }
            }

            return 0;
        }

        public int InsertForm(string name, string phone, string tc, string il, string ilce, string adress, string dogumTarih)
        {
            try
            {
                using (SqlConnection openCon = new SqlConnection("Data Source=94.138.197.30;Initial Catalog=farmasiuyelik;User ID=farmasiuyelik;Password=Sananeamk54;"))
                {
                    string saveStaff = $"INSERT into Customers (name,phone,tc,il,ilce,adres, dogumTarihi) VALUES ('{name}','{phone}','{tc}','{il}','{ilce}','{adress}','{dogumTarih}')";

                    using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {
                        querySaveStaff.Connection = openCon;
                        openCon.Open();
                        querySaveStaff.ExecuteNonQuery();
                        openCon.Close();
                    }
                }

                return 1;
            }
            catch (Exception exception)
            {
                using (SqlConnection openCon = new SqlConnection("Data Source=94.138.197.30;Initial Catalog=farmasiuyelik;User ID=farmasiuyelik;Password=Sananeamk54;"))
                {
                    string saveStaff = $"INSERT into Logs (logs,saved_date) VALUES ('{exception.Message}','{DateTime.Now}')";

                    using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {
                        querySaveStaff.Connection = openCon;
                        openCon.Open();
                        querySaveStaff.ExecuteNonQuery();
                        openCon.Close();
                    }
                }
                return 0;
            }
        }

        public void PostMail(string to,string name, string phone, string tc, string il, string ilce, string adress, string dogumTarih)
        {
            MailMessage ePosta = new MailMessage();

            //Kimden
            ePosta.From = new MailAddress("info@farmasi-uyekaydi.com");

            //Kime
            ePosta.To.Add(to);

            //Konu
            ePosta.Subject = "Farmasi Üye Kayıt";

            ePosta.IsBodyHtml = true;

            //İçerik
            ePosta.Body = String.Format(@"<html>
                      <body>
                          <p>Yeni kayıt,</p>
                          <p>Ad : {0}</p>
                          <p>Telefon : {1}</p>
                          <p>TC : {2}</p>
                          <p>İl : {3}</p>
                          <p>İlçe : {4}</p>
                          <p>Adres : {5}</p>
                          <p>Doğum Tarihi : {6}</p>
                      </body>
                      </html>", name,phone,tc,il,ilce,adress,dogumTarih);

            SmtpClient smtp = new SmtpClient();

            smtp.Credentials = new System.Net.NetworkCredential("info@farmasi-uyekaydi.com", "Sananeamk54");
            smtp.Port = 587;
            smtp.Host = "mail.farmasi-uyekaydi.com";
            smtp.EnableSsl = false;
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.Send(ePosta);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
            }
        }
    }
}