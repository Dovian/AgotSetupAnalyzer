using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerDB
{
    public class DBWriter : IDbWriter
    {
        public Task<IEnumerable<string>> UpdateDb(IEnumerable<Card> cards)
        {
            throw new NotImplementedException();
        }
    }
}
