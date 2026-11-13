using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

public class EformUser : IdentityUser<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Locale { get; set; }

    public bool IsGoogleAuthenticatorEnabled { get; set; }

    public string GoogleAuthenticatorSecretKey { get; set; }

    public virtual ICollection<EformUserRole> UserRoles { get; set; }
    public bool DarkTheme { get; set; }
    public string ThemeVariant { get; set; } = "eform";
    /// <summary>
    /// How this account picks its appearance: "system" follows the operating system and
    /// keeps following it, "light" and "dark" are fixed choices. DarkTheme remains the
    /// fallback appearance for consumers that predate this column.
    /// </summary>
    // Keep the "= system". New accounts get "system" from this initialiser, which EF writes
    // explicitly on every INSERT (a string has no default-derived sentinel, unlike IsActive).
    // The store-side DEFAULT in the mapping is what fills EXISTING rows with "system" when
    // the column is added, and the backfill migration keys on that marker.
    public string ThemeMode { get; set; } = "system";
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
    public string ProfilePicture { get; set; }
    public string ProfilePictureSnapshot { get; set; }
    /// <summary>
    /// False blocks sign-in for this account, a resigned employee say. Not the same as
    /// Identity's lockout, which is the automatic brute-force delay.
    /// </summary>
    // Keep the "= true". The mapping's HasDefaultValue(true) makes true this property's
    // sentinel, and EF Core omits a sentinel-valued column from the INSERT so the store
    // default applies. Without the initializer a new EformUser holds false, EF Core writes
    // that explicitly, and every account would be created disabled. (That is precisely what
    // happens to the shadow property ExternalLoginEnabled today, which cannot have one.)
    public bool IsActive { get; set; } = true;
}