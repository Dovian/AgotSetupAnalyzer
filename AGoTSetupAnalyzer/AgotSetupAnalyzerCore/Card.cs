using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        public Dictionary<string, object> AttachmentRestrictions { get; set; }
        public bool UsedAsDupe { get; set; }
        public bool UsedInSetup { get; set; }

        public bool Avoid { get; set; }
        public bool Never { get; set; }
        public bool Economy { get; set; }
        public bool Key { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Card card = obj as Card;
            if ((System.Object)card == null)
                return false;

            return card.CardCode == this.CardCode;
        }

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
        public Dictionary<string, object> AttachmentRestriction()
        {
            var result = new Dictionary<string, object>()
                {
                    {"General", ""},
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
                        result["Faction"] = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
                }
                if (Regex.IsMatch(restrictionPortion, "<i>.*</i>"))
                {
                    foreach (Match match in Regex.Matches(restrictionPortion, "<i>.*</i>"))
                        ((List<string>)result["Traits"]).Add(match.Value.Replace("<i>", string.Empty).Replace("</i>", string.Empty));
                }
                if (restrictionPortion.Contains("Unique"))
                    result["Unique"] = true;
                //How to parse on names?
            }

            else if (this.Type == StaticValues.Cardtypes.Character)
            {
                var restrictionPortion = Regex.Match(this.Text, "No attachments.*\\.").Value;
                if (restrictionPortion.Equals("No attachments."))
                    result["General"] = "NO ATTACHMENTS";
                else
                {
                    if (Regex.IsMatch(restrictionPortion, "<i>.*</i>"))
                    {
                        foreach (Match match in Regex.Matches(restrictionPortion, "<i>.*</i>"))
                            ((List<string>)result["Traits"]).Add(match.Value.Replace("<i>", string.Empty).Replace("</i>", string.Empty));
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

            return result;
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
            character.AttachmentRestrictions = character.AttachmentRestriction();
            attachment.AttachmentRestrictions = attachment.AttachmentRestriction();

            if (((string)character.AttachmentRestrictions["General"]) == "NO ATTACHMENTS")
                return false;

            if (((List<string>)character.AttachmentRestrictions["Traits"]).Count > 0)
                if (!((List<string>)character.AttachmentRestrictions["Traits"]).Any(s => attachment.Traits.Contains(s)))
                    return false;

            if (((List<string>)attachment.AttachmentRestrictions["Traits"]).Count > 0)
                if (!((List<string>)attachment.AttachmentRestrictions["Traits"]).Any(s => character.Traits.Contains(s)))
                    return false;

            if (!string.IsNullOrEmpty((string)attachment.AttachmentRestrictions["Faction"])
                && ((string)attachment.AttachmentRestrictions["Faction"]) != character.Faction)
                return false;

            if (((bool)attachment.AttachmentRestrictions["Unique"]) && !character.Unique)
                return false;


            return true;
        }

        public bool CanDupe(Card compareTo)
        {
            return (compareTo.Unique && this.Unique && string.Equals(this.Name, compareTo.Name));
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
