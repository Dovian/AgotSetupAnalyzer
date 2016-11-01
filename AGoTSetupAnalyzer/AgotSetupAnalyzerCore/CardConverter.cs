using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
                CardCode = json["code"] == null ? null : json["code"].ToString(),
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
                SetCode = json["pack_code"] == null ? "" : json["pack_code"].ToString(),
                Strength = json["strength"] == null ? 0 : int.Parse(json["strength"].ToString()),
                Text = json["text"] == null ? "" : json["text"].ToString(),
                ThronesDBUrl = json["url"] == null ? "" : json["url"].ToString(),
                Type = json["type_name"] == null
                    ? StaticValues.Cardtypes.None
                    : (StaticValues.Cardtypes)Enum.Parse(typeof(StaticValues.Cardtypes), json["type_name"].ToString()),
                Unique = json["is_unique"] == null ? false : bool.Parse(json["is_unique"].ToString()),
                Traits = json["traits"] == null
                ? null
                : json["traits"].ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray(),
            };
        }

        public static Card LocalDBDataToCard(DataTable table)
        {
            var columns = table.Columns;
            var fakeJToken = new Dictionary<string, object>();

            var deck = new List<Card>();
            foreach (DataRow row in table.Rows)
            {

                for (int i = 0; i < columns.Count; i++)
                    fakeJToken.Add(columns[i].ToString(), row[i]);
            }
            return new Card()
            {
                CardCode = fakeJToken["cardCode"] == null ? null : fakeJToken["cardCode"].ToString(),
                Cost = fakeJToken["cost"] == null ? 0 : int.Parse(fakeJToken["cost"].ToString()),
                Faction = fakeJToken["faction"] == null ? "" : fakeJToken["faction"].ToString(),
                ImageSource = fakeJToken["imageSource"] == null ? "" : fakeJToken["imageSource"].ToString(),
                Intrigue = fakeJToken["intrigue"] == null ? false : bool.Parse(fakeJToken["intrigue"].ToString()),
                Limited = fakeJToken["text"] == null ? false : fakeJToken["text"].ToString().Contains("Limited."),
                Loyal = fakeJToken["loyal"] == null ? false : bool.Parse(fakeJToken["loyal"].ToString()),
                Military = fakeJToken["military"] == null ? false : bool.Parse(fakeJToken["military"].ToString()),
                Name = fakeJToken["name"] == null ? "" : fakeJToken["name"].ToString(),
                PackCode = fakeJToken["packCode"] == null ? "" : fakeJToken["packCode"].ToString(),
                Power = fakeJToken["power"] == null ? false : bool.Parse(fakeJToken["power"].ToString()),
                SetCode = fakeJToken["setCode"] == null ? "" : fakeJToken["setCode"].ToString(),
                Strength = fakeJToken["strength"] == null ? 0 : int.Parse(fakeJToken["strength"].ToString()),
                Text = fakeJToken["text"] == null ? "" : fakeJToken["text"].ToString(),
                ThronesDBUrl = fakeJToken["thronesDBUrl"] == null ? "" : fakeJToken["thronesDBUrl"].ToString(),
                Type = fakeJToken["type"] == null
                ? StaticValues.Cardtypes.None
                : (StaticValues.Cardtypes)Enum.Parse(typeof(StaticValues.Cardtypes), fakeJToken["type"].ToString()),
                Unique = fakeJToken["uniqueCard"] == null ? false : bool.Parse(fakeJToken["uniqueCard"].ToString()),
                Traits = fakeJToken["traits"] == null ? null : (string[])fakeJToken["traits"]
            };
        }
    }
}
