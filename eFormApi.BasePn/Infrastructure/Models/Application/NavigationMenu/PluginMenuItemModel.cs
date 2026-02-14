namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application.NavigationMenu;

using System.Collections.Generic;

public class PluginMenuItemModel
{
    public string E2EId { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }
    public MenuItemTypeEnum Type { get; set; }
    public int Position { get; set; }
    public PluginMenuTemplateModel MenuTemplate { get; set; }

    public List<PluginMenuItemModel> ChildItems
        = new List<PluginMenuItemModel>();
    public List<PluginMenuTranslationModel> Translations { get; set; }
        = new List<PluginMenuTranslationModel>();

    public bool IsInternalLink { get; set; }
}