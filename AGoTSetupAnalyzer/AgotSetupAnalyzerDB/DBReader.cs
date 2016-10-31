using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerDB
{
    public class DBReader : IDbReader
    {
        public Task<string> GetCard(string cardName, string setCode = "")
        {
            throw new NotImplementedException();
        }
    }
}
