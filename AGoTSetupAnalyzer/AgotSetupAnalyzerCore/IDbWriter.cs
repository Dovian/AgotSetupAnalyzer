using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IDbWriter
    {
        Task<string> UpdateCards(IEnumerable<Card> cards);

        Task<string> AddToDb(IEnumerable<Card> cards);

        void LogToDb(string message);
    }
}
