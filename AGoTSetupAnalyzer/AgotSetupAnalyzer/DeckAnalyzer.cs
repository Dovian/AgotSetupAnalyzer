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
                if (!Results.TimesCardUsedInSetup.ContainsKey(card.ImageSource))
                    Results.TimesCardUsedInSetup.Add(card.ImageSource, 0);

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

                if (chosenSetup.CardsInHand.Count < config.CardFloorForGoodSetup
                    || chosenSetup.CharactersSetup() < config.CharacterFloorForGoodSetup
                    || (config.RequireEconomy && !(chosenSetup.NumOfEconCards() > 0))
                    || (config.RequireGreatCharacter && !chosenSetup.ContainsGreatCharacter()))
                {
                    if (config.MulliganAllPoorSetups)
                    {
                        deck.Shuffle();
                        hand = deck.DeckList.Take(7);
                        var mulliganSetup = TestAllSetups(hand.ToList(), config, true);


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
                        Results.UpdateResults(chosenSetup);
                    }
                }
                else
                    Results.UpdateResults(chosenSetup);
            }

            Results.Finalize(config.NumberOfTrials);
            return Results;
        }

        private SetupCards PickForMostCardsUsed(List<Card> hand, bool mulligan = false)
        {
            var goldRemaining = StaticValues.SetupGold;
            SetupCards setup = new SetupCards();
            setup.IsMulligan = mulligan;

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
                        card.UsedAsDupe = true;
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
                        card.UsedAsDupe = true;
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

            foreach (Card card in attachmentOptions)
            {
                if (!(card.Limited && setup.LimitedInSetup()))
                {
                    if (setup.CardsInHand.Any(c => c.Name == card.Name)
                            && card.CanDupe(setup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                    {
                        card.UsedAsDupe = true;
                        setup.CardsInHand.Add(card);
                    }
                    else if (goldRemaining >= card.Cost)
                    {
                        var possibleCharacters = setup.CardsInHand.Where(c => c.Type == StaticValues.Cardtypes.Character);
                        if (possibleCharacters.Any(c => card.CanAttach(c)))
                        {
                            goldRemaining -= card.Cost;
                            setup.CardsInHand.Add(card);
                        }
                    }
                    else
                        break;
                }
            }
            return setup;
        }

        private SetupCards TestAllSetups(List<Card> hand, AnalyzerConfigurationDTO config, bool mulligan = false)
        {
            SetupCards bestSetup = new SetupCards();

            var handCopy = hand.Select(c => Card.Clone(c)).OrderBy(c => c.Cost).ToList();
            var characterOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Character);
            var locationOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Location);
            var attachmentOptions = handCopy.Where(c => c.Type == StaticValues.Cardtypes.Attachment);

            var startingOptions = characterOptions.Distinct();

            foreach (Card startCard in startingOptions)
            {
                var goldRemaining = StaticValues.SetupGold;

                SetupCards currentSetup = new SetupCards();
                currentSetup.IsMulligan = mulligan;
                goldRemaining -= startCard.Cost;
                currentSetup.CardsInHand.Add(startCard);
                startCard.UsedInSetup = true;

                foreach (Card card in characterOptions)
                {
                    if (!(card.Limited && currentSetup.LimitedInSetup())
                        && !card.UsedInSetup
                        && !card.Never)
                    {
                        if (currentSetup.CardsInHand.Any(c => c.Name == card.Name)
                                && card.CanDupe(currentSetup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                        {
                            card.UsedAsDupe = true;
                            currentSetup.CardsInHand.Add(card);
                        }
                        else if (goldRemaining >= card.Cost)
                        {
                            goldRemaining -= card.Cost;
                            currentSetup.CardsInHand.Add(card);
                        }
                    }
                }

                foreach (Card card in locationOptions)
                {
                    if (!(card.Limited && currentSetup.LimitedInSetup())
                        && !card.UsedInSetup
                        && !card.Never)
                    {
                        if (currentSetup.CardsInHand.Any(c => c.Name == card.Name)
                                && card.CanDupe(currentSetup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                        {
                            card.UsedAsDupe = true;
                            currentSetup.CardsInHand.Add(card);
                        }
                        else if (goldRemaining >= card.Cost)
                        {
                            goldRemaining -= card.Cost;
                            currentSetup.CardsInHand.Add(card);
                        }
                    }
                }

                foreach (Card card in attachmentOptions)
                {
                    if (!(card.Limited && currentSetup.LimitedInSetup())
                        && !card.UsedInSetup
                        && !card.Never)
                    {
                        if (currentSetup.CardsInHand.Any(c => c.Name == card.Name)
                                && card.CanDupe(currentSetup.CardsInHand.Where(c => c.Name == card.Name).FirstOrDefault()))
                        {
                            card.UsedAsDupe = true;
                            currentSetup.CardsInHand.Add(card);
                        }
                        else if (goldRemaining >= card.Cost)
                        {
                            var possibleCharacters = currentSetup.CardsInHand.Where(c => c.Type == StaticValues.Cardtypes.Character);
                            if (possibleCharacters.Any(c => card.CanAttach(c)))
                            {
                                goldRemaining -= card.Cost;
                                currentSetup.CardsInHand.Add(card);
                            }
                        }
                    }
                }

                currentSetup.CalculateScore(config);

                if (currentSetup.SetupScore > bestSetup.SetupScore
                    || bestSetup.CardsInHand.Count == 0)
                    bestSetup = currentSetup;

                startCard.UsedInSetup = false;
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
    }
}
