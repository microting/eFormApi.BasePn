﻿namespace Microting.eFormApi.BasePn.Models.Settings
{
    public class SettingsModel
    {
        public ConnectionStringMainModel ConnectionStringMain { get; set; }
        public ConnectionStringSDKModel ConnectionStringSdk { get; set; }
        public AdminSetupModel AdminSetupModel { get; set; }
    }
}