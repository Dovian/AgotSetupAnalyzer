using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public class Deck
    {
        public List<Card> DeckList { get; set; }

        public Deck()
        {
            DeckList = new List<Card>();
        }

        public void Shuffle()
        {
            Random rand = new Random();
            List<Card> ShuffledDeck = new List<Card>();
            int DeckLength = DeckList.Count;
            int randSpot;

            for (int i = 0; i < DeckLength; i++)
            {
                randSpot = rand.Next(DeckList.Count);
                ShuffledDeck.Add(DeckList[randSpot]);

                DeckList.RemoveAt(randSpot);
            }

            DeckList = ShuffledDeck;
        }
    }
}
