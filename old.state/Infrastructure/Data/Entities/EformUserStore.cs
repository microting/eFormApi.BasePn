using Microsoft.AspNet.Identity.EntityFramework;

namespace Microting.eFormApi.BasePn.Infrastructure.Data.Entities
{
    public class EformUserStore : UserStore<EformUser, EformRole, int,
        EformUserLogin, EformUserRole, EformUserClaim>
    {
        public EformUserStore(BaseDbContext context)
            : base(context)
        {
        }
    }
}