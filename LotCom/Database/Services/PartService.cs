using LotCom.Core.Models;
using LotCom.Database.Auth;
using LotCom.Database.Mappers;
using LotCom.Database.Transfer;
using Newtonsoft.Json;

namespace LotCom.Database.Services;

public static class PartService
{
    /// <summary>
    /// Provides mapping methods.
    /// </summary>
    private static PartMapper _mapper = new PartMapper();

    /// <summary>
    /// Retrieves a single Part from the database using its Id.
    /// </summary>
    /// <param name="id">The Part object's Id number.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<Part?> Get(int id, HttpClient Client, UserAgent Agent)
    {
        // configure and execute the API call
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Part/{id}");
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
        PartDto? Dto = JsonConvert.DeserializeObject<PartDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Part from the response.");
        }
        return await _mapper.DtoToModel(Dto, Client, Agent);
    }

    /// <summary>
    /// Retrieves all Part objects printed by a Process, indicated by Id.
    /// </summary>
    /// <param name="id">The Id of the Process to collect printable Parts for.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Part>?> GetPrintedByProcess(int ProcessId, HttpClient Client, UserAgent Agent)
    {
        // configure and execute the API call
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Part/printedById?processId={ProcessId}");
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
        // deserialize the JSON response and map the data from Dtos to Models
        IEnumerable<PartDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<PartDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize any Parts from the response.");
        }
        IEnumerable<Part> Parts = await Task.WhenAll
        (
            Dtos.Select(async x => await _mapper.DtoToModel(x, Client, Agent))
        );
        return Parts;
    }

    /// <summary>
    /// Retrieves all Part objects scanned by a Process, indicated by Id.
    /// </summary>
    /// <param name="id">The Id of the Process to collect scannable Parts for.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Part>?> GetScannedByProcess(int ProcessId, HttpClient Client, UserAgent Agent)
    {
        // configure and execute the API call
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Part/scannedById?processId={ProcessId}");
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
        // deserialize the JSON response and map the data from Dtos to Models
        IEnumerable<PartDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<PartDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize any Parts from the response.");
        }
        IEnumerable<Part> Parts = await Task.WhenAll
        (
            Dtos.Select(async x => await _mapper.DtoToModel(x, Client, Agent))
        );
        return Parts;
    }
}