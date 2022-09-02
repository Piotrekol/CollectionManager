using CollectionManager.DataTypes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollectionManagerExtensionsDll.Modules.API.StreamCompanion
{
    public class StreamCompanionApi
    {
        private readonly HttpClient httpClient = new HttpClient();

        public Task<SCMapStats> GetMapStatsAsync(string osuFileLocation, Mods mods)
        {
            var strMods = "HR";

            if (mods != Mods.Nm)
                strMods = mods.ToString().Replace(",", "").Replace(" ", "").ToUpperInvariant();

            return GetMapStatsAsync(osuFileLocation, strMods);
        }

        public async Task<SCMapStats> GetMapStatsAsync(string osuFileLocation, string mods)
        {
            var statsUrl = $"http://localhost:20727/mapStats?osuFile={osuFileLocation}";
            if (!string.IsNullOrEmpty(mods))
                statsUrl += $"&mods={mods}";

            HttpResponseMessage result;
            try
            {
                result = await httpClient.GetAsync(statsUrl).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return null;
            }

            if (!result.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<SCMapStats>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync("http://localhost:20727/ping").ConfigureAwait(false);
                return response.Contains("pong");
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
