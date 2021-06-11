using Newtonsoft.Json;
using Xamarin.Essentials;
using Tarkov_price_check_app.Models;
using System.Collections.Generic;

namespace Tarkov_price_check_app.Services
{
    public class NameListHandler : INameListHandler
    {
        public List<ItemsListData> SavedList
        {
            get
            {
                var savedList = Deserialize<List<ItemsListData>>(Preferences.Get(nameof(SavedList), null));
                return savedList ?? new List<ItemsListData>();
            }
            set
            {
                var serializedList = Serialize(value);
                if (!string.IsNullOrEmpty(serializedList)) {
                Preferences.Set(nameof(SavedList), serializedList);
                }
            }
        }

        static T Deserialize<T>(string serializedObject) => JsonConvert.DeserializeObject<T>(serializedObject);

        static string Serialize<T>(T objectToSerialize) => JsonConvert.SerializeObject(objectToSerialize);
    }
}
