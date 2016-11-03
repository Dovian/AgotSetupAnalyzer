using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerCore
{
    public class AnalyzerResultsDTO
    {
        public int[] GoldUsed { get; set; }
        public int[] CardsUsed { get; set; }
        public int[] CharactersUsed { get; set; }
        public int[] NumOfEconCards { get; set; }
        public int BadSetups { get; set; }
        public int NumWithGreatCharacter { get; set; }
        public int Mulligans { get; set; }
        public Dictionary<string, int> TotalCharWithIcon { get; set; }
        public Dictionary<string, int> TotalStrPerIcon { get; set; }
        public Dictionary<string, int> TimesCardUsedInSetup { get; set; }

        public AnalyzerResultsDTO()
        {
            GoldUsed = new int[9];
            CardsUsed = new int[8];
            CharactersUsed = new int[8];
            NumOfEconCards = new int[8];
            TotalCharWithIcon = new Dictionary<string, int>(){
                {"Military", 0},
                {"Intrigue", 0},
                {"Power", 0}
            };
            TotalStrPerIcon = new Dictionary<string, int>(){
                {"Military", 0},
                {"Intrigue", 0},
                {"Power", 0}
            };
        }

        public void UpdateResults(SetupCards setup)
        {
            BadSetups += setup.IsBad ? 1 : 0;
            GoldUsed[setup.GoldUsed]++;
            CardsUsed[setup.CardsInHand.Count]++;
            CharactersUsed[setup.CharactersSetup]++;
            NumOfEconCards[setup.NumOfEconCards()]++;
            NumWithGreatCharacter += setup.ContainsGreatCharacter() ? 1 : 0;
            Mulligans += setup.IsMulligan ? 1 : 0;
            TotalCharWithIcon["Military"] += setup.IconsInSetup()["Military"] ? 1 : 0;
            TotalCharWithIcon["Intrigue"] += setup.IconsInSetup()["Intrigue"] ? 1 : 0;
            TotalCharWithIcon["Power"] += setup.IconsInSetup()["Power"] ? 1 : 0;
            TotalStrPerIcon["Military"] += setup.StrengthPerIcon()["Military"];
            TotalStrPerIcon["Intrigue"] += setup.StrengthPerIcon()["Intrigue"];
            TotalStrPerIcon["Power"] += setup.StrengthPerIcon()["Power"];
        }
    }
}