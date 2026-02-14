namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application;

public class ApplicationSettings
{
    public string DefaultLocale { get; set; }
    public string SiteLink { get; set; }
    public string SecurityCode { get; set; }
    public string DefaultPassword { get; set; }
    public bool IsTwoFactorForced { get; set; }
    public bool IsUserbackWidgetEnabled { get; set; }
    public string UserbackToken { get; set; }
}