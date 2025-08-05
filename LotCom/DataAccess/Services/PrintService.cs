using LotCom.Types;
using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Services;

/// <summary>
/// Provides access to the Print Database.
/// </summary>
public static class PrintService
{
    /// <summary>
    /// Retrieves a single Print from the Database using its Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<Print?> Get(int id, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Print/{id}");
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
        PrintDto? Dto = JsonConvert.DeserializeObject<PrintDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Print from the response.");
        }
        return await PrintMapper.DtoToModel(Dto, Agent);
    }

    /// <summary>
    /// Retrieves all Prints produced by ProcessId on Date from the Database.
    /// </summary>
    /// <param name="Date"></param>
    /// <param name="ProcessId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Print>?> GetOnDateByProcess(DateTime Date, int ProcessId, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"http://localhost:60000/Print/onDateBy?" + 
            $"day={Date.Day}" +
            $"&month={Date.Month}" +
            $"&year={Date.Year}" +
            $"&processId={ProcessId}"
        );
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
        IEnumerable<PrintDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<PrintDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Prints from the response.");
        }
        IEnumerable<Print> Prints = await Task.WhenAll
        (
            Dtos.Select(async x => await PrintMapper.DtoToModel(x, Agent))
        );
        return Prints;
    }
}