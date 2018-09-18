﻿using System.ComponentModel.DataAnnotations;

namespace Microting.eFormApi.BasePn.Models.Auth
{
    public class LoginModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }

        public string Code { get; set; }
    }
}