using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using LotCom.Types;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Services;

public static class ProcessService
{
    /// <summary>
    /// Retrieves all Processes from the database.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Process>?> GetAll(UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync("http://localhost:60000/Process");
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
        IEnumerable<ProcessDao>? Daos = JsonConvert.DeserializeObject<IEnumerable<ProcessDao>>(JSON);
        if (Daos is null)
        {
            throw new JsonException("Could not deserialize Processes from the response.");
        }
        IEnumerable<Process> Processes = await Task.WhenAll
        (
            Daos.Select(async x => await ProcessMapper.DaoToModel(x, Agent))
        );
        return Processes;
    }

    /// <summary>
    /// Retrieves a single Process from the database using its Id.
    /// </summary>
    /// <param name="id">The Part object's Id number.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<Process?> Get(int id, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Process/{id}");
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
        ProcessDao? Dao = JsonConvert.DeserializeObject<ProcessDao>(JSON);
        if (Dao is null)
        {
            throw new JsonException("Could not deserialize a Process from the response.");
        }
        return await ProcessMapper.DaoToModel(Dao, Agent);
    }
}