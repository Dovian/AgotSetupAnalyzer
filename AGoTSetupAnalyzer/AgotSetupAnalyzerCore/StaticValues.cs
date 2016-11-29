using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public static class StaticValues
    {
        public static int MaxSetSize = 211;
        public static int SetupGold = 8;

        public enum Cardtypes
        {
            None,
            Agenda,
            Plot,
            Title,
            Character,
            Attachment,
            Location,
            Event,
        }

        public static Dictionary<string, string> SetNameToSetCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"Valyrian Draft Set", "VDS"},
            {"Core Set", "Core"},
            {"Taking the Black", "TtB"},
            {"The Road to Winterfell", "TRtW"},
            {"The King's Peace", "TKP"},
            {"No Middle Ground", "NMG"},
            {"Calm over Westeros", "CoW"},
            {"True Steel", "TS"},
            {"Wolves of the North", "WotN"},
            {"Across the Seven Kingdoms", "AtSK"},
            {"Called to Arms", "CtA"},
            {"For Family Honor", "FFH"},
            {"There Is My Claim", "TIMC"},
            {"Ghosts of Harrenhal", "GoH"},
            {"Tyrion's Chain", "TC"},
            {"Lions of Casterly Rock", "LoCR"}
        };

        #region Card Lists
        public static string[] EconomyCards =
        {
            "01028",
            "01039",
            "01040",
            "01056",
            "01059",
            "01074",
            "01080",
            "01089",
            "01093",
            "01094",
            "01099",
            "01110",
            "01118",
            "01131",
            "01133",
            "01152",
            "01156",
            "01170",
            "01175",
            "01182",
            "01188",
            "01194",
            "02007",
            "02010",
            "02026",
            "02105",
            "03015",
            "03039",
            "03042",
            "04017",
            "04021",
            "04034",
            "04042",
            "04057",
            "04058",
            "04072",
            "05017",
            "05020",
            "05042",
        };

        public static string[] NeverSetupCards =
        {
            "01035",
            "02006",
            "02034",
            "02055",
            "02071",
            "02077",
            "02088",
            "02102",
            "02116",
            "03021",
            "03025",
            "04026",
            "04048",
        };

        public static string[] AvoidSetupCards = 
        {
            "01028",
            "01057",
            "01095",
            "01103",
            "01112",
            "01129",
            "01141",
            "01148",
            "01189",
            "02043",
            "02051",
            "02086",
            "03009",
            "03037",
            "04025",
            "04035",
            "04036",
            "04042",
            "04047",
            "04051",
            "04069",
            "04077",
            "04113",
            "04115",
            "05003",
            "05009",
            "05012",
            "05016",
            "05037",
        };
        #endregion
    }
}