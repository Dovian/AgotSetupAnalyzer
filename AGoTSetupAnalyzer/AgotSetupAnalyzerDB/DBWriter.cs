using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace AgotSetupAnalyzerDB
{
    public class DBWriter : IDbWriter
    {
        private readonly ILocalDBConfig config;

        public DBWriter(ILocalDBConfig config)
        {
            this.config = config;
        }

        public async Task<int> UpdateDb(IEnumerable<Card> cards)
        {
            var rowsAffected = 0;

            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    config.Server, config.Port, config.User, config.Password, config.Database);
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            foreach (Card card in cards)
            {
                string sql = ("INSERT INTO \"agotCardTable\" ("
                    + "\"cardCode\",cost,type,name,text,"
                    + "faction,loyal,\"packCode\",limited,\"uniqueCard\","
                    + "strength,military,intrigue,power,\"imageSource\","
                    + "\"thronesDBUrl\",traits)"
                    + " VALUES ("
                    + ":cardCode,:cost,:type,:name,:text,"
                    + ":faction,:loyal,:packCode,:limited,:uniqueCard,"
                    + ":strength,:military,:intrigue,:power,:imageSource,"
                    + ":thronesDBUrl,:traits)");

                NpgsqlCommand sqlCommand = new NpgsqlCommand(sql, connection);
                sqlCommand.Parameters.Add(new NpgsqlParameter("cardCode", card.CardCode));
                sqlCommand.Parameters.Add(new NpgsqlParameter("cost", card.Cost));
                sqlCommand.Parameters.Add(new NpgsqlParameter("type", card.Type.ToString()));
                sqlCommand.Parameters.Add(new NpgsqlParameter("name", card.Name));
                sqlCommand.Parameters.Add(new NpgsqlParameter("text", card.Text));
                sqlCommand.Parameters.Add(new NpgsqlParameter("faction", card.Faction));
                sqlCommand.Parameters.Add(new NpgsqlParameter("loyal", card.Loyal));
                sqlCommand.Parameters.Add(new NpgsqlParameter("packCode", card.PackCode));
                sqlCommand.Parameters.Add(new NpgsqlParameter("limited", card.Limited));
                sqlCommand.Parameters.Add(new NpgsqlParameter("uniqueCard", card.Unique));
                sqlCommand.Parameters.Add(new NpgsqlParameter("strength", card.Strength));
                sqlCommand.Parameters.Add(new NpgsqlParameter("military", card.Military));
                sqlCommand.Parameters.Add(new NpgsqlParameter("intrigue", card.Intrigue));
                sqlCommand.Parameters.Add(new NpgsqlParameter("power", card.Power));
                sqlCommand.Parameters.Add(new NpgsqlParameter("imageSource", card.ImageSource));
                sqlCommand.Parameters.Add(new NpgsqlParameter("thronesDBUrl", card.ThronesDBUrl));
                sqlCommand.Parameters.Add(new NpgsqlParameter("traits", card.Traits));

                try
                {
                    rowsAffected += await sqlCommand.ExecuteNonQueryAsync();
                }
                catch(PostgresException ex)
                {

                }
            }

            connection.Close();

            return rowsAffected;
        }
    }
}
