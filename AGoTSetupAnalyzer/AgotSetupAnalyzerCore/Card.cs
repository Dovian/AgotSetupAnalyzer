using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    [Serializable]
    public class Card
    {
        public string CardCode { get; set; }
        public int Cost { get; set; }
        public StaticValues.Cardtypes Type { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Traits { get; set; }
        public string Faction { get; set; }
        public bool Loyal { get; set; }
        public string PackCode { get; set; }

        public bool Limited { get; set; }
        public bool Unique { get; set; }

        public int Strength { get; set; }
        public bool Military { get; set; }
        public bool Intrigue { get; set; }
        public bool Power { get; set; }

        public string ImageSource { get; set; }
        public string ThronesDBUrl { get; set; }

        /*
         * Potentially need method to differentiate between inclusive and exclusive restrictions
         *  -Won't know how until we see what the wording ends up being
         * 
         * ATTACHMENTS
         * 1. Parse '*only.' out first and use that string for restrictions
         * 2. Find trait/faction by their unique surrounding strings 
         * 3. Search specifically for "Unique"
         * 4. Find a way to parse on names
         * 
         * CHARACTERS
         * 1. Parse 'No attachments*.' out first and use that string for restrictions
         * 2. So far it's safe to just parse for the Weapon trait, but it should be setup more generally and be capable of handling factions
         */
        public Dictionary<string, object> AttachmentRestriction
        {
            get;
            set
            {
                var restrictions = new Dictionary<string, object>()
                {
                    {"Traits", new List<string>()},
                    {"Faction", ""},
                    {"Unique", false}
                };

                if (this.Type == StaticValues.Cardtypes.Attachment)
                {
                    var restrictionPortion = Regex.Match(this.Text, ".*only\\.").Value;

                    if (Regex.IsMatch(restrictionPortion, "\\[.*\\]"))
                    {
                        foreach (Match match in Regex.Matches(restrictionPortion, "\\[.*\\]"))
                            restrictions["Faction"] = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
                    }
                    if (Regex.IsMatch(restrictionPortion, "<i>.*</i>"))
                    {
                        foreach (Match match in Regex.Matches(restrictionPortion, "<i>.*</i>"))
                            ((List<string>)restrictions["Traits"]).Add(match.Value.Replace("<i>", string.Empty).Replace("</i>", string.Empty));
                    }
                    if (restrictionPortion.Contains("Unique"))
                        restrictions["Unique"] = true;
                    //How to parse on names?
                }

                else if (this.Type == StaticValues.Cardtypes.Character)
                {
                    var restrictionPortion = Regex.Match(this.Text, "No attachments.*\\.").Value;
                    if (restrictionPortion.Equals("No attachments."))
                        restrictions.Add("General", "NO ATTACHMENTS");
                    else
                    {
                        if (Regex.IsMatch(restrictionPortion, "<i>.*</i>"))
                        {
                            foreach (Match match in Regex.Matches(restrictionPortion, "<i>.*</i>"))
                                ((List<string>)restrictions["Traits"]).Add(match.Value.Replace("<i>", string.Empty).Replace("</i>", string.Empty));
                        }
                        /* Handle when it actually exists
                        if (Regex.IsMatch(restrictionPortion, "\\[.*\\]"))
                        {
                            foreach (Match match in Regex.Matches(restrictionPortion, "\\[.*\\]"))
                                restrictions.Add(match.Value.Replace("[", string.Empty).Replace("]", string.Empty));
                        }
                         */
                    }
                }

                value = restrictions;
            }
        }

        public bool CanAttach(Card compareTo)
        {
            Card character;
            Card attachment;

            if (compareTo.Type == StaticValues.Cardtypes.Character && this.Type == StaticValues.Cardtypes.Attachment)
            {
                character = compareTo;
                attachment = this;
            }
            else if (compareTo.Type == StaticValues.Cardtypes.Attachment && this.Type == StaticValues.Cardtypes.Character)
            {
                character = this;
                attachment = compareTo;
            }
            else
            {
                return false;
            }

            if (character.AttachmentRestriction["General"] == "NO ATTACHMENTS")
                return false;

            if (((List<string>)character.AttachmentRestriction["Traits"]).Count > 0)
                if (!((List<string>)character.AttachmentRestriction["Traits"]).Any(s => attachment.Traits.Contains(s)))
                    return false;

            if (((List<string>)attachment.AttachmentRestriction["Traits"]).Count > 0)
                if (!((List<string>)attachment.AttachmentRestriction["Traits"]).Any(s => character.Traits.Contains(s)))
                    return false;

            if (((string)attachment.AttachmentRestriction["Faction"]) != character.Faction)
                return false;

            if (((bool)attachment.AttachmentRestriction["Unique"]) && !character.Unique)
                return false;


            return true;
        }

        public bool CanDupe(Card compareTo)
        {
            return (compareTo.Unique && this.Unique && string.Equals(this.Name, compareTo.Name));
        }
    }
}
