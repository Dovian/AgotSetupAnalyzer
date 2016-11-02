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
        public bool IsBad { get; set; }

        public Dictionary<string, bool> IconsInSetup()
        {
            return new Dictionary<string, bool>(){
                {"Military", CardsInHand.Any(c => c.Military)},
                {"Intrigue", CardsInHand.Any(c => c.Intrigue)},
                {"Power", CardsInHand.Any(c => c.Power)},
            };
        }

        public bool LimitedInSetup()
        {
            return CardsInHand.Any(c => c.Limited);
        }

        public int NumOfCharacters()
        {
            return CardsInHand.Count(c => c.Type == StaticValues.Cardtypes.Character);
        }

        public bool ContainsEcon()
        {
            bool result = false;

            foreach (Card c in CardsInHand)
                if (StaticValues.EconomyCards.Contains(c.CardCode))
                    result = true;

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
