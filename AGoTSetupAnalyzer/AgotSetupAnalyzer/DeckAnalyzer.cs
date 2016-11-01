using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzer
{
    public class DeckAnalyzer : IDeckAnalyzer
    {
        private readonly IDatabaseProvider dbProvider;

        public DeckAnalyzer(IDatabaseProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public async Task<AnalyzerResultsDTO> Analyze(AnalyzerConfigurationDTO config)
        {
            var cardNames = ParseThronesDbList(config.DeckList);
            Deck deck = new Deck()
            {
                DeckList = (await dbProvider.PopulateDecklist(cardNames)).ToList()
            };

            deck.Shuffle();
            var hand = deck.DeckList.Take(7);

            return new AnalyzerResultsDTO();
        }

        //Likely best sort for most used
        private List<Card> PickForMostCardsUsed(List<Card> hand)
        {
            var goldRemaining = StaticValues.SetupGold;
            List<Card> setup = new List<Card>();

            var handCopy = hand.OrderBy(c => c.Cost).ToList();
            
            while (goldRemaining > 0)
            {
                goldRemaining -= handCopy.First().Cost;

                if (goldRemaining >= 0)
                    setup.Concat(handCopy.Take(1));
            }

            /*Check replacements (does goldRemaining + cost = another character's cost?)*/

            return setup;
        }


        //Possible sort for closest to 8
        private List<Card> PickForClosestToCap(List<Card> hand)
        {
            var goldRemaining = StaticValues.SetupGold;
            List<Card> setup = new List<Card>();
            var possibleSetups = new List<List<Card>>();


            for (int i = 7; i > 0; i--)
            {
                var handCopy = hand.OrderBy(c => c.Cost);
                if (handCopy.Any(c => c.Cost == i))
                {
                    var possibleSetup = new List<Card>();
                    possibleSetup.Add(handCopy.First(c => c.Cost == i));

                    goldRemaining -= i;

                    while (goldRemaining > 0)
                    {
                        goldRemaining -= handCopy.First().Cost;

                        if (goldRemaining >= 0)
                            possibleSetup.Concat(handCopy.Take(1));
                    }

                    possibleSetups.Add(possibleSetup);
                }

                goldRemaining = 8;
            }

            /*Pick from possibles*/

            return setup;
        }

        private string[] ParseThronesDbList(string thronesDbList)
        {
            var lines = thronesDbList.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<int> linesToRemove = new List<int>();
            for (int i = 0; i < lines.Count; i++)
                if (lines[i].First() != '1' && lines[i].First() != '2' && lines[i].First() != '3')
                    linesToRemove.Add(i);

            linesToRemove = linesToRemove.OrderByDescending(l => l).ToList();
            foreach (int line in linesToRemove)
                lines.RemoveAt(line);

            return lines.ToArray();
        }
    }
}
