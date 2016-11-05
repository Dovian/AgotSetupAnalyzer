using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public class SetupCards
    {
        public List<Card> CardsInHand { get; set; }
        public bool IsMulligan { get; set; }
        public bool IsBad { get; set; }
        public int CharactersSetup { get; set; }

        public SetupCards()
        {
            CardsInHand = new List<Card>();
        }

        public int GoldUsed()
        {
            return CardsInHand.Where(c => !c.UsedAsDupe).Sum(c => c.Cost);
        }

        public Dictionary<string, double> IconsInSetup()
        {
            return new Dictionary<string, double>(){
                {"Military", CardsInHand.Where(c => c.Military && !c.UsedAsDupe).Count()},
                {"Intrigue", CardsInHand.Where(c => c.Intrigue && !c.UsedAsDupe).Count()},
                {"Power", CardsInHand.Where(c => c.Power && !c.UsedAsDupe).Count()},
            };
        }

        public Dictionary<string, double> StrengthPerIcon()
        {
            return new Dictionary<string, double>(){
                {"Military", CardsInHand.Where(c => c.Military && !c.UsedAsDupe).Sum(c => c.Strength)},
                {"Intrigue", CardsInHand.Where(c => c.Intrigue && !c.UsedAsDupe).Sum(c => c.Strength)},
                {"Power", CardsInHand.Where(c => c.Power && !c.UsedAsDupe).Sum(c => c.Strength)},
            };
        }

        public bool LimitedInSetup()
        {
            return CardsInHand.Any(c => c.Limited);
        }

        public int NumOfEconCards()
        {
            int result = 0;

            foreach (Card c in CardsInHand)
                if (StaticValues.EconomyCards.Contains(c.CardCode) && !c.UsedAsDupe)
                    result++;

            return result;

        }

        public bool ContainsGreatCharacter()
        {
            bool result = false;

            foreach (Card c in CardsInHand)
                if (c.Cost >= 4 && c.Type == StaticValues.Cardtypes.Character)
                    result = true;

            return result;
        }
    }
}
