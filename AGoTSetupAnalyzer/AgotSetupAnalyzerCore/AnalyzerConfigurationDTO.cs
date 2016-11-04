using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerCore
{
    public class AnalyzerConfigurationDTO
    {
        [DisplayName("Deck List:")]
        public string DeckList { get; set; }
        [DisplayName("Number of trials to run:")]
        public int NumberOfTrials { get; set; }
        [DisplayName("Minimum number of cards for a good setup:")]
        public int CardFloorForGoodSetup { get; set; }
        [DisplayName("Minimum number of characters for a good setup:")]
        public int CharacterFloorForGoodSetup { get; set; }

        //"Great" Refers to 4 or higher cost
        [DisplayName("Require 4-cost or higher character:")]
        public bool RequireGreatCharacter { get; set; }
        [DisplayName("Prefer 4-cost or higher character:")]
        public bool PreferGreatCharacter { get; set; }

        //Checked by "EconomyCards" in StaticValues
        [DisplayName("Require economy card:")]
        public bool RequireEconomy { get; set; }
        [DisplayName("Prefer economy card:")]
        public bool PreferEconomy { get; set; }

        [DisplayName("Mulligan all setups considered 'poor':")]
        public bool MulliganAllPoorSetups { get; set; }
        [DisplayName("Mulligan all setups without a key card:")]
        public bool MulliganNoKey { get; set; }
        [DisplayName("Mulligan all setups without economy:")]
        public bool MulliganNoEcon { get; set; }
    }
}