﻿namespace Microting.eFormApi.BasePn.Infrastructure.Models.Auth
{
    public class GoogleAuthInfoModel
    {
        public string PSK { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public bool IsTwoFactorForced { get; set; }
    }
}