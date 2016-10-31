using AgotSetupAnalyzerCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCore
{
    [TestClass]
    public class TestCore
    {
        [TestMethod]
        public void TestShuffle()
        {
            var decklist = new Deck();
            var unshuffledDeck = decklist;

            for (int i = 0; i < 60; i++)
                decklist.DeckList.Add(new Card()
                {
                    Cost = i,
                    Name = i.ToString()
                });

            decklist.Shuffle();

            Assert.IsNotNull(decklist.DeckList);
        }
    }
}
