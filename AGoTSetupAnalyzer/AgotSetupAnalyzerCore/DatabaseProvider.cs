using AgotSetupAnalyzerCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IThronesDBProvider thronesDbProvider;
        private readonly IDbReader localDbReader;
        private readonly IDbWriter localDbWriter;

        public DatabaseProvider(IThronesDBProvider thronesDbProvider,
            IDbReader localDbReader,
            IDbWriter localDbWriter)
        {
            this.thronesDbProvider = thronesDbProvider;
            this.localDbReader = localDbReader;
            this.localDbWriter = localDbWriter;
        }

        public async Task<int> InitialDBPopulation()
        {
            var thronesDbCards = await thronesDbProvider.GetAllCards();

            var JsonArrayCards = JArray.Parse(thronesDbCards);
            var convertedResults = JsonArrayCards.Select(c => CardConverter.ThronesDBDataToCard(c)).AsEnumerable();

            return await localDbWriter.UpdateDb(convertedResults);
        }

        public async Task<int> UpdateDB(int SetCode)
        {
            var codeList = new List<string>();
            for (int i = 1; i < StaticValues.MaxSetSize; i++)
            {
                codeList.Add(String.Format("{0:D2}{1:D3}", SetCode, i));
            }
            var JsonArrayCards = new JArray();

            foreach (string code in codeList)
            {
                var thronesDbCard = await thronesDbProvider.GetSingleCard(code);
                JsonArrayCards.Add(JToken.Parse(thronesDbCard));
            }
            var convertedResults = JsonArrayCards.Select(c => CardConverter.ThronesDBDataToCard(c)).AsEnumerable();

            return await localDbWriter.UpdateDb(convertedResults);
        }

        public async Task<IEnumerable<Card>> PopulateDecklist(string[] CardNames)
        {
            var DeckList = new List<Card>();

            foreach (string card in CardNames)
            {
                int quantity;
                string cardName;
                string setCode = "";

                List<string> splitName = new List<string>();
                splitName.Add(card.Substring(0, 2));
                splitName = splitName.Concat(card.Remove(0,2).Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)).ToList();

                for (int i = 0; i < splitName.Count; i++)
                {
                    splitName[i] = splitName[i].Trim();
                    splitName[i] = splitName[i].Replace(")", string.Empty);
                }

                quantity = int.Parse(splitName[0][0].ToString());
                cardName = splitName[1];
                if (splitName.Count == 3)
                {
                    if (StaticValues.SetNameToSetCode.ContainsKey(splitName[2]))
                        setCode = StaticValues.SetNameToSetCode[splitName[2]];
                    else
                        setCode = splitName[2];
                }

                var localDbCard = await localDbReader.GetCard(cardName, setCode);
                var convertedResult = CardConverter.LocalDBDataToCard(localDbCard);

                for (int i = 0; i < quantity; i++)
                {
                    DeckList.Add(Card.Clone(convertedResult));
                }
            }

            return DeckList;
        }
    }
}
