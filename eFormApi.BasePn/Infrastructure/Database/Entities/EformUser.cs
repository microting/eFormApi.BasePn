using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities
{
    public class EformUser : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Locale { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }

        public string GoogleAuthenticatorSecretKey { get; set; }

        public virtual ICollection<EformUserRole> UserRoles { get; set; }

        public bool DarkTheme { get; set; }

        public string TimeZone { get; set; }

        public string Formats { get; set; }

        public string ArchiveSoftwareVersion { get; set; }

        public string ArchiveModel { get; set; }

        public string ArchiveManufacturer { get; set; }

        public string ArchiveOsVersion { get; set; }

        public string ArchiveLastIp { get; set; }

        public string ArchiveLastKnownLocation { get; set; }

        public string ArchiveLookedUpIp { get; set; }

        public string TimeRegistrationSoftwareVersion { get; set; }

        public string TimeRegistrationModel { get; set; }

        public string TimeRegistrationManufacturer { get; set; }

        public string TimeRegistrationOsVersion { get; set; }

        public string TimeRegistrationLastIp { get; set; }

        public string TimeRegistrationLastKnownLocation { get; set; }

        public string TimeRegistrationLookedUpIp { get; set; }

        public string EmailSha256 { get; set; }
    }
}