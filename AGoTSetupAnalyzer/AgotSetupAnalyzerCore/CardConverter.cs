using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public static class CardConverter
    {
        public static Card ThronesDBDataToCard(JToken json)
        {
            return new Card()
            {
                Cost = json["cost"] == null ? 0 : int.Parse(json["cost"].ToString()),
                Faction = json["faction_code"] == null ? "" : json["faction_code"].ToString(),
                ImageSource = json["imagesrc"] == null ? "" : json["imagesrc"].ToString(),
                Intrigue = json["is_intrigue"] == null ? false : bool.Parse(json["is_intrigue"].ToString()),
                Limited = json["text"] == null ? false : json["text"].ToString().Contains("Limited."),
                Loyal = json["is_loyal"] == null ? false : bool.Parse(json["is_loyal"].ToString()),
                Military = json["is_military"] == null ? false : bool.Parse(json["is_military"].ToString()),
                Name = json["name"] == null ? "" : json["name"].ToString(),
                PackCode = json["pack_code"] == null ? "" : json["pack_code"].ToString(),
                Power = json["is_power"] == null ? false : bool.Parse(json["is_power"].ToString()),
                Set = json["pack_name"] == null ? "" : json["pack_name"].ToString(),
                Strength = json["strength"] == null ? 0 : int.Parse(json["strength"].ToString()),
                Text = json["text"] == null ? "" : json["text"].ToString(),
                ThronesDBUrl = json["url"] == null ? "" : json["url"].ToString(),
                Type = json["type_name"] == null
                    ? StaticValues.Cardtypes.None
                    : (StaticValues.Cardtypes)Enum.Parse(typeof(StaticValues.Cardtypes), json["type_name"].ToString()),
                Unique = json["is_unique"] == null ? false : bool.Parse(json["is_unique"].ToString()),
            };
        }

        public static Card LocalDBDataToCard(object obj)
        {
            return new Card();
        }
    }
}
