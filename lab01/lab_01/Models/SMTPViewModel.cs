using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab_01.Models
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
            Host = configuration["SMTPSettings:Host"];
            Port = Convert.ToInt32(configuration["SMTPSettings:Port"]);
            EmailAddress = configuration["SMTPSettings:Email"];
            Login = configuration["SMTPSettings:Login"];
            Password = configuration["SMTPSettings:Password"];
        }
        
    }
}
