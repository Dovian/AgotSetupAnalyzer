using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool CanDupe(Card compareTo)
        {
            return (compareTo.Unique && this.Unique && string.Equals(this.Name, compareTo.Name));
        }
    }
}
