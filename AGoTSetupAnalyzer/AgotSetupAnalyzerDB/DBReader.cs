using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace AgotSetupAnalyzerDB
{
    public class DBReader : IDbReader
    {
        private readonly ILocalDBConfig config;

        public DBReader(ILocalDBConfig config)
        {
            this.config = config;
        }

        public async Task<DataTable> GetCard(string cardName, string setCode = "")
        {
            DataSet result = new DataSet();

            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    config.Server, config.Port, config.User, config.Password, config.Database);

            string sql = String.Format("SELECT * FROM {0} WHERE cardName='{1}'", config.Database, cardName);
            if (!string.IsNullOrEmpty(setCode))
                sql += String.Format(" AND setCode='{0}'", setCode);

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, connection);

            adapter.Fill(result);

            connection.Close();

            return result.Tables[0];
        }
    }
}
