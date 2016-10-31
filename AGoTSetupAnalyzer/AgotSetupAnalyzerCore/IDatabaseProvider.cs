using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface IDatabaseProvider
    {
        Task<IEnumerable<string>> InitialDBPopulation();

        Task<IEnumerable<string>> UpdateDB(int SetCode);

        Task<IEnumerable<Card>> PopulateDecklist(string[] CardNames);
    }
}
