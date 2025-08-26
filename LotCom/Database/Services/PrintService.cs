using LotCom.Core.Models;
using LotCom.Database.Auth;
using LotCom.Database.Balancing;
using LotCom.Database.Mappers;
using LotCom.Database.Transfer;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LotCom.Database.Services;

/// <summary>
/// Provides access to the Print Database.
/// </summary>
public static class PrintService
{
    /// <summary>
    /// Provides mapping methods.
    /// </summary>
    private static PrintMapper _mapper = new PrintMapper();

    /// <summary>
    /// Provides balancing for expensive processing.
    /// </summary>
    private static PrintBalancer _balancer = new PrintBalancer();

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
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Print/{id}");
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
        PrintDto? Dto = JsonConvert.DeserializeObject<PrintDto>(JSON);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Print from the response.");
        }
        return await _mapper.DtoToModel(Dto, Agent);
    }

    /// <summary>
    /// Retrieves all Prints produced by ProcessId on Date from the Database.
    /// </summary>
    /// <param name="Date"></param>
    /// <param name="ProcessId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Print>?> GetOnDateByProcess(DateTime Date, int ProcessId, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"https://lotcom.yna.us/api/Print/onDateBy?" +
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
        IEnumerable<PrintDto>? Dtos = JsonConvert.DeserializeObject<IEnumerable<PrintDto>>(JSON);
        if (Dtos is null)
        {
            throw new JsonException("Could not deserialize Prints from the response.");
        }
        // convert the DTOs to Models using a balanced process
        IEnumerable<Print> Prints = await _balancer.ConvertUsingChunking(Dtos, _mapper, Agent);
        return Prints;
    }

    /// <summary>
    /// Sends a Print to the Database to be inserted.
    /// </summary>
    /// <param name="Model"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Create(Print Model, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        // convert the Model into Dto
        PrintDto Dto = _mapper.ModelToDto(Model);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PostAsync
        (
            $"https://lotcom.yna.us/api/Print",
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
    /// Updates an existing Print entity in the Database, using TargetId and NewModel as the source. 
    /// </summary>
    /// <param name="TargetId"></param>
    /// <param name="NewModel"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Update(int TargetId, Print NewModel, UserAgent Agent)
    {
        HttpClient Client = HttpClientFactory.Create(Agent);
        // convert the Model into Dto
        PrintDto Dto = _mapper.ModelToDto(NewModel);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PutAsync
        (
            $"https://lotcom.yna.us/api/Print/{TargetId}",
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