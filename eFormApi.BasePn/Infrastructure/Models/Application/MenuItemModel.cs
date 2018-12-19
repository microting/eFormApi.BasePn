using System.Collections.Generic;

namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application
{
    public class MenuItemModel
    {
        public string Name { get; set; }
        public string LocaleName { get; set; }
        public string Link { get; set; }
        public string E2EId { get; set; }
        public int Position { get; set; }

        public List<MenuItemModel> MenuItems { get; set; }
            = new List<MenuItemModel>();

        public List<string> Guards { get; set; }
            = new List<string>();
    }
}