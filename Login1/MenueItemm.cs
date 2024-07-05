using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login1
{
    public class MenuItemm
    {
        public int MenuId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public string ImageUrl { get; set; }
        public string RestaurantName { get; set; }  // Add this line
    }

    public class MenuItemGroup : List<MenuItemm>
    {
        public string Name { get; private set; }

        public MenuItemGroup(string name, IEnumerable<MenuItemm> items) : base(items)
        {
            Name = name;
        }
    }
}

