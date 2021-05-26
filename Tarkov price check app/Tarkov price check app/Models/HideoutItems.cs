using System.Collections.Generic;

namespace Tarkov_price_check_app.Models
{
    public class HideoutItems
    {
        public enum Stations
        {
            Intel,
            Lav,
            Work
        }

        public class Ingredients
        {
            public List<string> Ingredient { get; set; }
            public List<int> IngredientAmount { get; set; }
        }

        public class Item
        {
            public Stations Station { get; set; }
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
