using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using LotCom.Types;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Services;

public static class PartService
{
    /// <summary>
    /// Retrieves a single Part from the database using its Id.
    /// </summary>
    /// <param name="id">The Part object's Id number.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<Part?> Get(int id, UserAgent Agent)
    {
        // configure a new HttpClient and execute the API call
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Part/{id}");
        // ensure that the response was OK and retrieve its contents as JSON
        try
        {
            Response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        string JSON = await Response.Content.ReadAsStringAsync();
        // deserialize the JSON response and map the data from Dao to Model
        PartDao? Dao = JsonConvert.DeserializeObject<PartDao>(JSON);
        if (Dao is null)
        {
            throw new JsonException("Could not deserialize a Part from the response.");
        }
        return PartMapper.DaoToModel(Dao);
    }

    /// <summary>
    /// Retrieves all Part objects printed by a Process, indicated by Id.
    /// </summary>
    /// <param name="id">The Id of the Process to collect printable Parts for.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Part>?> GetPrintedByProcess(int ProcessId, UserAgent Agent)
    {
        // configure a new HttpClient and execute the API call
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Part/printedById?processId={ProcessId}");
        // ensure that the response was OK and retrieve its contents as JSON
        try
        {
            Response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        string JSON = await Response.Content.ReadAsStringAsync();
        // deserialize the JSON response and map the data from Daos to Models
        IEnumerable<PartDao>? Daos = JsonConvert.DeserializeObject<IEnumerable<PartDao>>(JSON);
        if (Daos is null)
        {
            throw new JsonException("Could not deserialize any Parts from the response.");
        }
        return Daos.Select(PartMapper.DaoToModel);
    }

    /// <summary>
    /// Retrieves all Part objects scanned by a Process, indicated by Id.
    /// </summary>
    /// <param name="id">The Id of the Process to collect scannable Parts for.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Part>?> GetScannedByProcess(int ProcessId, UserAgent Agent)
    {
        // configure a new HttpClient and execute the API call
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Part/scannedById?processId={ProcessId}");
        // ensure that the response was OK and retrieve its contents as JSON
        try
        {
            Response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        string JSON = await Response.Content.ReadAsStringAsync();
        // deserialize the JSON response and map the data from Daos to Models
        IEnumerable<PartDao>? Daos = JsonConvert.DeserializeObject<IEnumerable<PartDao>>(JSON);
        if (Daos is null)
        {
            throw new JsonException("Could not deserialize any Parts from the response.");
        }
        return Daos.Select(PartMapper.DaoToModel);
    }
}