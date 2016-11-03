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

            for (int i = 0; i < config.NumberOfTrials; i++)
            {
                deck.Shuffle();
                var hand = deck.DeckList.Take(7);
                var chosenSetup = PickForMostCardsUsed(hand.ToList());

                if (chosenSetup.CardsInHand.Count < config.CardFloorForGoodSetup
                    || chosenSetup.CardsInHand.Where(c => c.Type == StaticValues.Cardtypes.Character).Count() < config.CharacterFloorForGoodSetup
                    || (config.RequireEconomy && !chosenSetup.ContainsEcon())
                    || (config.RequireGreatCharacter && !chosenSetup.ContainsGreatCharacter()))
                    chosenSetup.IsBad = true;
            }

            return new AnalyzerResultsDTO();
        }

        //Likely best sort for most used
        private SetupCards PickForMostCardsUsed(List<Card> hand)
        {
            var goldRemaining = StaticValues.SetupGold;
            SetupCards setup = new SetupCards();

            var handCopy = hand.OrderBy(c => c.Cost).ToList();
            var characterOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Character);
            var locationOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Location);
            var attachmentOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Attachment);

            foreach (Card card in characterOptions)
            {
                if (!(card.Limited && setup.LimitedInSetup()))
                {
                    if (setup.CardsInHand.Any(c => c.Name == card.Name)
                            && card.CanDupe(setup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                    {
                        setup.CardsInHand.Add(card);
                    }
                    else if (goldRemaining >= card.Cost)
                    {
                        goldRemaining -= card.Cost;
                        setup.CardsInHand.Add(card);
                    }
                    else
                        break;
                }
            }

            foreach (Card card in locationOptions)
            {
                if (!(card.Limited && setup.LimitedInSetup()))
                {
                    if (setup.CardsInHand.Any(c => c.Name == card.Name)
                            && card.CanDupe(setup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                    {
                        setup.CardsInHand.Add(card);
                    }
                    else if (goldRemaining >= card.Cost)
                    {
                        goldRemaining -= card.Cost;
                        setup.CardsInHand.Add(card);
                    }
                    else
                        break;
                }
            }

            /*These need special rules
            foreach (Card card in attachmentOptions)
            {
                if (!(card.Limited && setup.LimitedInSetup()))
                {
                    if (setup.CardsInHand.Any(c => c.Name == card.Name)
                            && card.CanDupe(setup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                    {
                        setup.CardsInHand.Add(card);
                    }
                    else if (goldRemaining >= card.Cost)
                    {
                        goldRemaining -= card.Cost;
                        setup.CardsInHand.Add(card);
                    }
                    else
                        break;
                }
            }*/

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
