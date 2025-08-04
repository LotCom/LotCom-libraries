using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using LotCom.Types;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Services;

public static class ProcessService
{
    
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
        // deserialize the JSON response and map the data from DTO to Model
        ProcessDto? Dto = JsonConvert.DeserializeObject<ProcessDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Process from the response.");
        }
        return await ProcessMapper.DtoToModel(Dto, Agent);
    }
}