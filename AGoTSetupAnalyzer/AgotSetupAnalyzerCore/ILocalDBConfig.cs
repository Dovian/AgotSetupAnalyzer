using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgotSetupAnalyzerCore
{
    public interface ILocalDBConfig
    {
        string Server { get; }
        string Port { get; }
        string User { get; }
        string Password { get; }
        string Database { get; }
    }
}
