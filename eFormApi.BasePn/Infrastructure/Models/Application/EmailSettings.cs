﻿namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string SendGridKey { get; set; }
    }
}