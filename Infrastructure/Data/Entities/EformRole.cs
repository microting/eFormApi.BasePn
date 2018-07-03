using Microsoft.AspNet.Identity.EntityFramework;

namespace eFormApi.BasePn.Infrastructure.Data.Entities
{
    public class EformRole : IdentityRole<int, EformUserRole>
    {
        public EformRole()
        {
        }

        public EformRole(string name)
        {
            Name = name;
        }
    }
}