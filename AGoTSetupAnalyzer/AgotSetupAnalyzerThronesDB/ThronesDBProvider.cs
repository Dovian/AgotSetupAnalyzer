using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace AgotSetupAnalyzerThronesDB
{
    public class ThronesDBProvider : IThronesDBProvider
    {
        public async Task<string> GetAllCards()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://thronesdb.com/api/public/cards/");

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSingleCard(string CardCode)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://thronesdb.com/api/public/card/" + CardCode);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
