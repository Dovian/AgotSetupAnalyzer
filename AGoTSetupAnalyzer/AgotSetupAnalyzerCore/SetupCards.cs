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
        public int GoldUsed { get; set; }
        public int CharactersSetup { get; set; }

        public SetupCards()
        {
            CardsInHand = new List<Card>();
        }

        public Dictionary<string, bool> IconsInSetup()
        {
            return new Dictionary<string, bool>(){
                {"Military", CardsInHand.Any(c => c.Military)},
                {"Intrigue", CardsInHand.Any(c => c.Intrigue)},
                {"Power", CardsInHand.Any(c => c.Power)},
            };
        }

        public Dictionary<string, int> StrengthPerIcon()
        {
            return new Dictionary<string, int>(){
                {"Military", CardsInHand.Where(c => c.Military).Count()},
                {"Intrigue", CardsInHand.Where(c => c.Intrigue).Count()},
                {"Power", CardsInHand.Where(c => c.Power).Count()},
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
                if (StaticValues.EconomyCards.Contains(c.CardCode))
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
