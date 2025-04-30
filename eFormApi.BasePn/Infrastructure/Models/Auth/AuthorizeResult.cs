using System;

namespace Microting.eFormApi.BasePn.Infrastructure.Models.Auth
{
    public class AuthorizeResult
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string UserName { get; set; }
        public bool IsFirstUser { get; set; }
    }
}