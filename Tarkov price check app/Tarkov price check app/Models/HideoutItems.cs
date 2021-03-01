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

        public class ResultItem
        {
            public string ResultItemName { get; set; }
            public int ResultProfit { get; set; }
            public int IngredientPrice { get; set; }
            public int ResultCount { get; set; }
            public Ingredients Ingredients { get; set; }
        }

        public class IntelCenter
        {
            public List<ResultItem> ResultItem { get; set; }
        }

        public class Lavatory
        {
            public List<ResultItem> ResultItem { get; set; }
        }

        public class Workbench
        {
            public List<ResultItem> ResultItem { get; set; }
        }

        public class FullItems
        {
            public IntelCenter IntelCenter { get; set; }
            public Lavatory Lavatory { get; set; }
            public Workbench Workbench { get; set; }
        }

        public class Root
        {
            public FullItems FullItems { get; set; }
        }
    }
}
