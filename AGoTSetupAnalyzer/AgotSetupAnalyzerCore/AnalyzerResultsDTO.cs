using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerCore
{
    public class AnalyzerResultsDTO
    {
        public double[] GoldUsed { get; set; }
        public double[] CardsUsed { get; set; }
        public double[] CharactersUsed { get; set; }
        public double[] NumOfEconCards { get; set; }
        public double BadSetups { get; set; }
        public double NumWithGreatCharacter { get; set; }
        public double Mulligans { get; set; }
        public Dictionary<string, double> TotalCharWithIcon { get; set; }
        public Dictionary<string, double> TotalStrPerIcon { get; set; }
        public Dictionary<string, double> TimesCardUsedInSetup { get; set; }

        public AnalyzerResultsDTO()
        {
            GoldUsed = new double[9];
            CardsUsed = new double[8];
            CharactersUsed = new double[8];
            NumOfEconCards = new double[8];
            TotalCharWithIcon = new Dictionary<string, double>(){
                {"Military", 0},
                {"Intrigue", 0},
                {"Power", 0}
            };
            TotalStrPerIcon = new Dictionary<string, double>(){
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
            TotalCharWithIcon["Military"] += setup.IconsInSetup()["Military"];
            TotalCharWithIcon["Intrigue"] += setup.IconsInSetup()["Intrigue"];
            TotalCharWithIcon["Power"] += setup.IconsInSetup()["Power"];
            TotalStrPerIcon["Military"] += setup.StrengthPerIcon()["Military"];
            TotalStrPerIcon["Intrigue"] += setup.StrengthPerIcon()["Intrigue"];
            TotalStrPerIcon["Power"] += setup.StrengthPerIcon()["Power"];
        }

        public void Finalize(int trials)
        {
            BadSetups /= trials;
            NumWithGreatCharacter /= trials;
            Mulligans /= trials;
            TotalCharWithIcon["Military"] /= trials;
            TotalCharWithIcon["Intrigue"] /= trials;
            TotalCharWithIcon["Power"] /= trials;
            TotalStrPerIcon["Military"] /= trials;
            TotalStrPerIcon["Intrigue"] /= trials;
            TotalStrPerIcon["Power"] /= trials;
            for (int i = 0; i < GoldUsed.Length; i++)
                GoldUsed[i] /= trials;

            for (int i = 0; i < CardsUsed.Length; i++)
                CardsUsed[i] /= trials;

            for (int i = 0; i < CharactersUsed.Length; i++)
                CharactersUsed[i] /= trials;

            for (int i = 0; i < NumOfEconCards.Length; i++)
                NumOfEconCards[i] /= trials;
        }
    }
}