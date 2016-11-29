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
        public int SetupScore { get; set; }
        public int GoldRemaining { get; set; }

        public SetupCards()
        {
            CardsInHand = new List<Card>();
            SetupScore = 0;
            GoldRemaining = StaticValues.SetupGold;
        }

        public int CharactersSetup()
        {
            return CardsInHand.Where(c => !c.UsedAsDupe && c.Type == StaticValues.Cardtypes.Character).Count();
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
            return CardsInHand.Where(c => c.Economy && !c.UsedAsDupe).Count();
        }

        public bool ContainsGreatCharacter()
        {
            bool result = false;

            foreach (Card c in CardsInHand)
                if (c.Cost >= 4 && c.Type == StaticValues.Cardtypes.Character && !c.UsedAsDupe)
                    result = true;

            return result;
        }

        public void CalculateScore(AnalyzerConfigurationDTO config)
        {
            if (config.RequireEconomy || config.PreferEconomy)
                if (this.NumOfEconCards() > 0)
                    this.SetupScore += 1000000;

            if (config.RequireGreatCharacter || config.PreferGreatCharacter)
                if (this.ContainsGreatCharacter())
                    this.SetupScore += 1000000;

            this.SetupScore += this.CardsInHand.Where(c => c.Key).Count() * 1000000;

            this.SetupScore += this.CardsInHand.Count() * 100000;
            this.SetupScore += this.GoldUsed() * 10000;
            
            this.SetupScore += (this.LimitedInSetup() ? 0 : 1) * 1000;
            this.SetupScore += this.CharactersSetup() * 100;
            this.SetupScore += this.IconsInSetup().Where(pair => pair.Value > 0).Count() * 10;
            this.SetupScore += (int)this.StrengthPerIcon().Sum(pair => pair.Value);

            this.SetupScore -= (this.CardsInHand.Where(c => c.Avoid).Count() * 100000);

            if (this.CharactersSetup() < config.CharacterFloorForGoodSetup ||
                this.CardsInHand.Count < config.CardFloorForGoodSetup ||
                (config.RequireGreatCharacter && !this.ContainsGreatCharacter()) ||
                (config.RequireEconomy && !(this.NumOfEconCards() > 0)))
                this.SetupScore = 0;
        }
    }
}
