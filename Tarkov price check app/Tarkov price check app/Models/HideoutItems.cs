using System.Collections.Generic;

namespace Tarkov_price_check_app.Models
{
    public class HideoutItems
    {
        public class Ingredients
        {
            public List<string> Ingredient { get; set; }
            public List<int> IngredientAmmount { get; set; }
        }

        public class Item
        {
            public string Station { get; set; }
            public string ResultItemName { get; set; }
            public int ResultProfit { get; set; }
            public int ResultCount { get; set; }
            public Ingredients Ingredients { get; set; }
        }

        public class FullItems
        {
            public List<Item> Items { get; set; }
        }
    }
}
