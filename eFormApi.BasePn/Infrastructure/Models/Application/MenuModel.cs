using System.Collections.Generic;

namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application;

public class MenuModel
{
    public List<MenuItemModel> LeftMenu { get; set; }
        = new List<MenuItemModel>();

    public List<MenuItemModel> RightMenu { get; set; }
        = new List<MenuItemModel>();
}