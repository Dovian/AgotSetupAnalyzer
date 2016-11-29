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
            var Results = new AnalyzerResultsDTO();
            var cardNames = ParseThronesDbList(config.DeckList);
            Deck deck = new Deck()
            {
                DeckList = (await dbProvider.PopulateDecklist(cardNames)).ToList()
            };

            foreach (Card card in deck.DeckList)
            {
                if (!Results.TimesCardUsedInSetup.ContainsKey(card.CardCode))
                    Results.TimesCardUsedInSetup.Add(card.CardCode, 0);

                if (StaticValues.NeverSetupCards.Contains(card.CardCode))
                    card.Never = true;
                if (StaticValues.EconomyCards.Contains(card.CardCode))
                    card.Economy = true;
            }

            for (int i = 0; i < config.NumberOfTrials; i++)
            {
                deck.Shuffle();
                var hand = deck.DeckList.Take(7);
                var chosenSetup = TestAllSetups(hand.ToList(), config);

                if (i % 100 == 0)
                    Results.CreateExampleHand(hand, chosenSetup.CardsInHand);

                if (chosenSetup.CardsInHand.Count < config.CardFloorForGoodSetup
                    || chosenSetup.CharactersSetup() < config.CharacterFloorForGoodSetup
                    || (config.RequireEconomy && !(chosenSetup.NumOfEconCards() > 0))
                    || (config.RequireGreatCharacter && !chosenSetup.ContainsGreatCharacter()))
                {
                    if (config.MulliganAllPoorSetups)
                    {

                        deck.Shuffle();
                        var mulliganHand = deck.DeckList.Take(7);
                        var mulliganSetup = TestAllSetups(mulliganHand.ToList(), config, true);

                        if (i % 100 == 0)
                            Results.CreateExampleHand(mulliganHand, mulliganSetup.CardsInHand, true);

                        if (mulliganSetup.CardsInHand.Count < config.CardFloorForGoodSetup
                            || mulliganSetup.CharactersSetup() < config.CharacterFloorForGoodSetup
                            || (config.RequireEconomy && !(mulliganSetup.NumOfEconCards() > 0))
                            || (config.RequireGreatCharacter && !mulliganSetup.ContainsGreatCharacter()))
                            mulliganSetup.IsBad = true;

                        Results.UpdateResults(mulliganSetup);
                    }
                    else
                    {
                        chosenSetup.IsBad = true;
                        if (i % 100 == 0)
                            Results.MulliganExampleHand.Add(new List<Tuple<string, bool>>());

                        Results.UpdateResults(chosenSetup);
                    }
                }
                else
                {
                    if (i % 100 == 0)
                        Results.MulliganExampleHand.Add(new List<Tuple<string, bool>>());

                    Results.UpdateResults(chosenSetup);
                }
            }

            Results.Finalize(config.NumberOfTrials);
            return Results;
        }

        private SetupCards TestAllSetups(List<Card> hand, AnalyzerConfigurationDTO config, bool mulligan = false)
        {
            SetupCards bestSetup = new SetupCards();
            var handCopy = hand.Select(c => Card.Clone(c)).OrderBy(c => c.Cost).ToList();

            var characterOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Character && !c.Avoid && !c.Never);
            var locationOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Location && !c.Avoid && !c.Never);
            var attachmentOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Attachment && !c.Avoid && !c.Never);

            var avoidedCharacters = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Character && c.Avoid && !c.Never);
            var avoidedLocations = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Location && c.Avoid && !c.Never);
            var avoidedAttachments = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Attachment && c.Avoid && !c.Never);

            var startingOptions = characterOptions.Distinct();

            foreach (Card startCard in startingOptions)
            {
                SetupCards currentSetup = new SetupCards();
                currentSetup.IsMulligan = mulligan;
                currentSetup.GoldRemaining -= startCard.Cost;
                currentSetup.CardsInHand.Add(startCard);
                startCard.UsedInSetup = true;

                GenericAddToSetup(characterOptions, ref currentSetup);

                if (currentSetup.CharactersSetup() <= config.CharacterFloorForGoodSetup)
                {
                    GenericAddToSetup(avoidedCharacters, ref currentSetup);
                }

                GenericAddToSetup(locationOptions, ref currentSetup);
                GenericAddToSetup(attachmentOptions, ref currentSetup);

                GenericAddToSetup(avoidedCharacters, ref currentSetup);
                GenericAddToSetup(avoidedLocations, ref currentSetup);
                GenericAddToSetup(avoidedAttachments, ref currentSetup);

                currentSetup.CalculateScore(config);

                if (currentSetup.SetupScore > bestSetup.SetupScore
                    || bestSetup.CardsInHand.Count == 0)
                    bestSetup = currentSetup;

                handCopy.ForEach(c => c.UsedInSetup = false);
            }

            return bestSetup;
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

        private void GenericAddToSetup(IEnumerable<Card> options, ref SetupCards setup)
        {
            foreach (Card card in options)
            {
                if (!(card.Limited && setup.LimitedInSetup())
                    && !card.UsedInSetup)
                {
                    if (setup.CardsInHand.Any(c => c.Name == card.Name)
                            && card.CanDupe(setup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                    {
                        card.UsedAsDupe = true;
                        card.UsedInSetup = true;
                        setup.CardsInHand.Add(card);
                    }
                    else if (setup.GoldRemaining >= card.Cost)
                    {
                        if (card.Type == StaticValues.Cardtypes.Attachment)
                        {
                            var possibleCharacters = setup.CardsInHand.Where(c => c.Type == StaticValues.Cardtypes.Character);
                            if (possibleCharacters.Any(c => card.CanAttach(c)))
                            {
                                setup.GoldRemaining -= card.Cost;
                                setup.CardsInHand.Add(card);
                                card.UsedInSetup = true;
                            }
                        }
                        else
                        {
                            setup.GoldRemaining -= card.Cost;
                            setup.CardsInHand.Add(card);
                            card.UsedInSetup = true;
                        }
                    }
                }
            }
        }
    }
}
