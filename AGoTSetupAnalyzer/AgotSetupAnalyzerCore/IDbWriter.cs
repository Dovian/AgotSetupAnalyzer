using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IDbWriter
    {
        Task<int> UpdateDb(IEnumerable<Card> cards);
    }
}
