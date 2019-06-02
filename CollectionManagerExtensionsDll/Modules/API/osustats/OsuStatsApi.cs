//#define TestServer
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.FileCollections;
using CollectionManager.Modules.FileIO.OsuDb;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CollectionManagerExtensionsDll.Modules.API.osustats
{
    public class OsuStatsApi : IWebCollectionProvider
    {
        private readonly MapCacher _mapCacher;
        private OsdbCollectionHandler collectionHandler = new OsdbCollectionHandler(null);
        private string _apiKey;
        public UserInformation UserInformation { get; private set; }

#if TestServer
        private const string baseUrl = "http://api.osustats.vue/";
#else
        private const string baseUrl = "https://osustats.ppy.sh/apiv2/";
#endif

        HttpClient httpClient = new HttpClient();
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                _apiKey = value;
                httpClient.DefaultRequestHeaders.Remove("ApiKey");
                httpClient.DefaultRequestHeaders.Add("ApiKey", value);
                UserInformation = null;
            }
        }

        public OsuStatsApi(string apiKey, MapCacher mapCacher)
        {
            _mapCacher = mapCacher;
            ApiKey = apiKey;
        }

        public bool CanFetch()
        {
            return UserInformation != null;
        }

        public async Task<bool> IsCurrentKeyValid()
        {
            return await UpdateUserInformation();
        }

        public async Task<bool> UpdateUserInformation()
        {
            var data = await httpClient.GetStringAsync($"{baseUrl}account/me");

            try
            {
                UserInformation = JsonConvert.DeserializeObject<UserInformation>(data);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fetches full blown osdb files with contain all collection metadata and all beatmaps
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<ICollection>> GetCollections(string path)
        {
            var stream = await httpClient.GetStreamAsync($"{baseUrl}{path}");

            var tempFile = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.osdb");

            using (var fileStream = File.Create(tempFile))
            {
                stream.CopyTo(fileStream);
            }

            return collectionHandler.ReadOsdb(tempFile, _mapCacher);
        }
        /// <summary>
        /// Fetches collection metadata without their beatmaps
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<WebCollection>> GetCollectionList(string path)
        {
            var stream = await httpClient.GetStringAsync($"{baseUrl}{path}");
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            var apiResponse = JsonConvert.DeserializeObject<ApiCollectionResponse>(stream, jsonSerializerSettings);
            var webCollections = new List<WebCollection>();
            foreach (var webCollectionToken in apiResponse.Data)
            {
                webCollections.Add(new WebCollection(path,webCollectionToken["id"].ToObject<int>(),_mapCacher)
                {
                    Name = webCollectionToken["title"].ToString(),
                    NumberOfBeatmaps = webCollectionToken["totalBeatmapsCount"].ToObject<int>()
                });
            }
            return webCollections;
        }

        public class ApiCollectionResponse
        {
            public JArray Data { get; set; }
            public int Total { get; set; }
            public int PerPage { get; set; }
        }
        /// <summary>
        /// Returns collection with given online Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public async Task<ICollection> GetCollection(int collectionId)
        {
            return (await GetCollections($"collection/{collectionId}/download")).First();
        }

        /// <summary>
        /// Returns all <see cref="WebCollection"/> that user identified by <see cref="ApiKey"/> has ever uploaded.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WebCollection>> GetMyCollectionList()
        {
            return await GetCollectionList("collection?all=1");
        }

        public async Task<ICollection> GetCollection(string path)
        {
            return (await GetCollections(path)).First();
        }

        public Task<bool> SaveCollection(ICollection collection)
        {
            throw new System.NotImplementedException();
        }
    }
}