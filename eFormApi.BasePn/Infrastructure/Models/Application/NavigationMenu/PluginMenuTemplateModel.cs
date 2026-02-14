namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application.NavigationMenu;

using System.Collections.Generic;

public class PluginMenuTemplateModel
{
    public string Name { get; set; }
    public string DefaultLink { get; set; }
    public string E2EId { get; set; }

    public List<PluginMenuTranslationModel> Translations
        = new List<PluginMenuTranslationModel>();
    public List<PluginMenuTemplatePermissionModel> Permissions { get; set; }
        = new List<PluginMenuTemplatePermissionModel>();
}