using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IDatabaseProvider
    {
        Task<string> InitialDBPopulation();

        Task<string> UpdateFullDb();

        Task<string> UpdateDBBySet(int SetCode);

        Task<IEnumerable<Card>> PopulateDecklist(string[] CardNames);

        void LogMessage(string message);
    }
}
