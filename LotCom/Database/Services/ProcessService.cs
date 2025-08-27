using LotCom.Core.Models;
using LotCom.Database.Auth;
using LotCom.Database.Mappers;
using LotCom.Database.Transfer;
using Newtonsoft.Json;

namespace LotCom.Database.Services;

public static class ProcessService
{
    /// <summary>
    /// Provides mapping methods.
    /// </summary>
    private static ProcessMapper _mapper = new ProcessMapper();

    /// <summary>
    /// Retrieves all Processes from the database.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Process>?> GetAll(HttpClient Client, UserAgent Agent)
    {
        HttpResponseMessage? Response = await Client.GetAsync("https://lotcom.yna.us/api/Process");
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
        // deserialize the JSON response and map the data from Dto to Model
        IEnumerable<ProcessDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<ProcessDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Processes from the response.");
        }
        IEnumerable<Process> Processes = await Task.WhenAll
        (
            Dtos.Select(async x => await _mapper.DtoToModel(x, Client, Agent))
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
    public static async Task<Process?> Get(int id, HttpClient Client, UserAgent Agent)
    {
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Process/{id}");
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
        // deserialize the JSON response and map the data from Dto to Model
        ProcessDto? Dto = JsonConvert.DeserializeObject<ProcessDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Process from the response.");
        }
        return await _mapper.DtoToModel(Dto, Client, Agent);
    }
}