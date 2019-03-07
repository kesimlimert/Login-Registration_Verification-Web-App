using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebApplication7;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        LibraryEntities db = new LibraryEntities();





        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveData(Kullanici model)
        {
            model.IsValid = false;
            string id = Guid.NewGuid().ToString();
            model.ID = id;
            db.Kullanicis.Add(model);
            db.SaveChanges();
            SendEmail(model);
            return Json("Kayit Tamamlandi", JsonRequestBehavior.AllowGet);


        }

        public ActionResult Confirm(string regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        public ActionResult RegisterConfirm(string regId)
        {
            Kullanici Data = db.Kullanicis.Where(x => x.ID == regId).FirstOrDefault();
            Data.IsValid = true;
            db.SaveChanges();
            var msg = "Emailiniz dogrulandi!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        public void SendEmail(Kullanici model)
        {
           
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.live.com";
            sc.EnableSsl = true;
            sc.Timeout = 50000;

            sc.Credentials = new System.Net.NetworkCredential("SENDERMAIL", "SENDERPASSWORD");

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("mertkesimli@hotmail.com", "Mert");

            db.Kullanicis.Where(y => y.ID == model.ID).FirstOrDefault();

            mail.To.Add(model.eposta);

            mail.Subject = "Lutfen mailinizi onaylayin";
            mail.IsBodyHtml = true;
           
            db.Kullanicis.Where(x => x.ID == model.ID).FirstOrDefault();
            var url = "http://localhost:52597/" + "Home/Confirm?regId=" + model.ID;
            mail.Body = url;
            sc.Send(mail);


        }
    }
}



