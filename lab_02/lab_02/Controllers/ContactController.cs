using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab_02.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab_02Controllers
{
    public class ContactController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Send(MessageViewModel message)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(message.EmailAddress, SMTPViewModel.Login));
                emailMessage.To.Add(new MailboxAddress("", SMTPViewModel.EmailAddress));
                emailMessage.Subject = message.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message.Text
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SMTPViewModel.Host, SMTPViewModel.Port, false);
                    client.Authenticate(SMTPViewModel.Login, SMTPViewModel.Password);
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }
            }
            catch(Exception ex)
            {
                ViewData["Message"] = "Ошибка при отправке!\n"+ex;
            }

            ViewData["Message"] = "Сообщение отправлено!";
            return View("Index");
        }
    }
}
