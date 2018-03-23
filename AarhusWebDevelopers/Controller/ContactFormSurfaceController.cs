using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevelopers.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevelopers.Controller
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

            MailMessage message = new MailMessage();
            message.To.Add("eaakdj@students.eaaa.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            comment.SetValue("messageName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("messageContent", model.Message);

            // save
            Services.ContentService.Save(comment);

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("babdummy@gmail.com", "neqzdlcyibbrwjgj");

                //send mail
                smtp.Send(message);

                TempData["success"] = true;
                
            }

            return RedirectToCurrentUmbracoPage();

        }
    }
}