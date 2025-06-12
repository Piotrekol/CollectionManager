//#define TestServer
namespace CollectionManager.Extensions.Modules.API.osustats;

using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class OsuStatsApi : IWebCollectionProvider, IDisposable
{
    private readonly MapCacher _mapCacher;
    private string _apiKey;

    private UserInformation _userInformation;

#if TestServer
    private const string baseUrl = "http://api.osustats.vue/";
#else
    private const string baseUrl = "https://osustats.ppy.sh/apiv2/";
#endif

    private readonly HttpClient httpClient = new();

    public IUserInformation UserInformation => (IUserInformation)_userInformation;
    public string ApiKey
    {
        get => _apiKey;
        set
        {
            _apiKey = value;
            _ = httpClient.DefaultRequestHeaders.Remove("ApiKey");
            httpClient.DefaultRequestHeaders.Add("ApiKey", value);
            _userInformation = null;
        }
    }

    public OsuStatsApi(string apiKey, MapCacher mapCacher)
    {
        _mapCacher = mapCacher;
        ApiKey = apiKey;
    }

    public bool CanFetch() => _userInformation != null;

    public async Task<bool> IsCurrentKeyValid()
    {
        if (string.IsNullOrEmpty(ApiKey) || !await UpdateUserInformation())
        {
            _userInformation = null;
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateUserInformation()
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}account/me");

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        try
        {
            _userInformation =
                JsonConvert.DeserializeObject<UserInformation>(await response.Content.ReadAsStringAsync());
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Fetches full blown osdb files with contain all collection metadata and all beatmaps
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<IOsuCollection>> GetCollections(string path)
    {
        Stream stream = await httpClient.GetStreamAsync($"{baseUrl}{path}");

        string tempFile = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.osdb");

        using (FileStream fileStream = File.Create(tempFile))
        {
            stream.CopyTo(fileStream);
        }

        return OsdbCollectionHandler.ReadOsdb(tempFile, _mapCacher);
    }

    public async Task<bool> RemoveCollection(int collectionId)
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"{baseUrl}collection/{collectionId}");

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    ///     Fetches collection metadata without their beatmaps
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<WebCollection>> FetchCollectionList(string path)
    {
        string stream = await httpClient.GetStringAsync($"{baseUrl}{path}");

        return GetCollectionList(stream);
    }

    /// <summary>
    ///     Fetches collection metadata without their beatmaps
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected IEnumerable<WebCollection> GetCollectionList(string jsonResponse)
    {
        JsonSerializerSettings jsonSerializerSettings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        ApiCollectionResponse apiResponse =
            JsonConvert.DeserializeObject<ApiCollectionResponse>(jsonResponse, jsonSerializerSettings);
        List<WebCollection> webCollections = [];
        foreach (JToken webCollectionToken in apiResponse.Data)
        {
            webCollections.Add(new WebCollection(webCollectionToken["id"].ToObject<int>(), _mapCacher)
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
    ///     Returns collection with given online Id
    /// </summary>
    /// <param name="collectionId"></param>
    /// <returns></returns>
    public async Task<IOsuCollection> GetCollection(int collectionId) => (await GetCollections($"collection/{collectionId}/download")).First();

    /// <summary>
    ///     Returns all <see cref="WebCollection" /> that user identified by <see cref="ApiKey" /> has ever uploaded.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<WebCollection>> GetMyCollectionList() => await FetchCollectionList($"collection?user={_userInformation.OsuUserId}&all=1");

    public async Task<IOsuCollection> GetCollection(string path) => (await GetCollections(path)).First();

    public async Task<IEnumerable<WebCollection>> SaveCollection(IOsuCollection collection)
    {
        using MemoryStream memoryStream = new();
        OsdbCollectionHandler.WriteOsdb([collection], memoryStream,
            collection.LastEditorUsername ?? "", true);
        memoryStream.Position = 0;

        using HttpContent osdbContent = new StreamContent(memoryStream);
        using MultipartFormDataContent httpContent = [];
        httpContent.Add(osdbContent, "file", $"{collection.Name}.osdb");
        HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}collection/import", httpContent);

        return GetCollectionList(await response.Content.ReadAsStringAsync());
    }

    public void Dispose()
    {
        httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}