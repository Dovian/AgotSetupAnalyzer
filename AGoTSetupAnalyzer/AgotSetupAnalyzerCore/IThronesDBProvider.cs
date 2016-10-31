using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IThronesDBProvider
    {
        Task<string> GetAllCards();

        Task<string> GetSingleCard(string CardCode);
    }
}
