using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab_02.Models
{
    public class SMTPViewModel
    {
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static string EmailAddress { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
       
        public static void LoadSettings(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Host = configuration["SMTP:Host"];
            Port = Convert.ToInt32(configuration["SMTP:Port"]);
            EmailAddress = configuration["SMTP:Email"];
            Login = configuration["SMTP:Login"];
            Password = configuration["SMTP:Password"];
        }
        
    }
}
