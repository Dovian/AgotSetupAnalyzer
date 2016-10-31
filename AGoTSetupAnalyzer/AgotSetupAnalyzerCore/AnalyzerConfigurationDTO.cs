using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerCore
{
    public class AnalyzerConfigurationDTO
    {
        public string DeckList { get; set; }

        public int NumberOfTrials { get; set; }
        public int CardFloorForGoodSetup { get; set; }
        public int CharacterFloorForGoodSetup { get; set; }

        //"Great" Refers to 4 or higher cost
        public bool RequireGreatCharacter { get; set; }
        public bool PreferGreatCharacter { get; set; }

        //Checked by "EconomyCards" in StaticValues
        public bool RequireEconomy { get; set; }
        public bool PreferEconomy { get; set; }

        public bool MulliganAllPoorSetups { get; set; }
        public bool MulliganNoKey { get; set; }
        public bool MulliganNoEcon { get; set; }
    }
}