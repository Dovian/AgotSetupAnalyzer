using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IDbReader
    {
        Task<DataTable> GetCard(string cardName, string setCode = "");
    }
}
