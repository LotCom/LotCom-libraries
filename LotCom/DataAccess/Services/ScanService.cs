using LotCom.Types;
using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LotCom.DataAccess.Services;

/// <summary>
/// Provides access to the Scan Database.
/// </summary>
public static class ScanService
{
    /// <summary>
    /// Retrieves all Scans from the Database.
    /// </summary>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAll(UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Scan");
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
        IEnumerable<ScanDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<ScanDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Scans from the response.");
        }
        IEnumerable<Scan> Scans = await Task.WhenAll
        (
            Dtos.Select(async x => await ScanMapper.DtoToModel(x, Agent))
        );
        return Scans;
    }
    
    /// <summary>
    /// Retrieves all Scans from the Database within a set range from the current date.
    /// </summary>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAllWithinRange(int WithinDaysOfCurrent, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Scan/within?days={WithinDaysOfCurrent}");
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
        IEnumerable<ScanDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<ScanDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Scans from the response.");
        }
        IEnumerable<Scan> Scans = await Task.WhenAll
        (
            Dtos.Select(async x => await ScanMapper.DtoToModel(x, Agent))
        );
        return Scans;
    }

    /// <summary>
    /// Retrieves a single Scan from the Database using its Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<Scan?> Get(int id, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync($"http://localhost:60000/Scan/{id}");
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
        ScanDto? Dto = JsonConvert.DeserializeObject<ScanDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Scan from the response.");
        }
        return await ScanMapper.DtoToModel(Dto, Agent);
    }

    /// <summary>
    /// Retrieves all Scans produced by ProcessId from the Database.
    /// </summary>
    /// <param name="ProcessId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAllForProcess(int ProcessId, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"http://localhost:60000/Scan/by?" +
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
        // deserialize the JSON response and map the data from Dto to Model
        IEnumerable<ScanDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<ScanDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Scans from the response.");
        }
        IEnumerable<Scan> Scans = await Task.WhenAll
        (
            Dtos.Select(async x => await ScanMapper.DtoToModel(x, Agent))
        );
        return Scans;
    }

    /// <summary>
    /// Retrieves all Scans produced by ProcessId on Date from the Database.
    /// </summary>
    /// <param name="Date"></param>
    /// <param name="ProcessId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetOnDateByProcess(DateTime Date, int ProcessId, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"http://localhost:60000/Scan/onDateBy?" +
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
        // deserialize the JSON response and map the data from Dto to Model
        IEnumerable<ScanDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<ScanDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Scans from the response.");
        }
        IEnumerable<Scan> Scans = await Task.WhenAll
        (
            Dtos.Select(async x => await ScanMapper.DtoToModel(x, Agent))
        );
        return Scans;
    }

    /// <summary>
    /// Sends a Scan to the Database to be inserted.
    /// </summary>
    /// <param name="Model"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Create(Scan Model, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        // convert the Model into Dto
        ScanDto Dto = ScanMapper.ModelToDto(Model);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PostAsync
        (
            $"http://localhost:60000/Scan",
            Content
        );
        // ensure that the response was okay
        try
        {
            Response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        // confirm that the response contained a Created status
        if (Response.StatusCode != System.Net.HttpStatusCode.Created)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Updates an existing Scan entity in the Database, using TargetId and NewModel as the source. 
    /// </summary>
    /// <param name="TargetId"></param>
    /// <param name="NewModel"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Update(int TargetId, Scan NewModel, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        // convert the Model into Dto
        ScanDto Dto = ScanMapper.ModelToDto(NewModel);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PutAsync
        (
            $"http://localhost:60000/Scan/{TargetId}",
            Content
        );
        // ensure that the response was okay
        try
        {
            Response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        if (Response.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}