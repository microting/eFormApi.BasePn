using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities
{
    public class EformRole : IdentityRole<int>
    {
        public const string Admin = "admin";
        
        public const string User = "user";

        public virtual ICollection<EformUserRole> UserRoles { get; set; }
    }
}