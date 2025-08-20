using LotCom.Types;
using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
        // deserialize the JSON response and map the data from Dao to Model
        PrintDao? Dao = JsonConvert.DeserializeObject<PrintDao>(JSON);
        if (Dao is null)
        {
            throw new JsonException("Could not deserialize a Print from the response.");
        }
        return await PrintMapper.DaoToModel(Dao, Agent);
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
        // deserialize the JSON response and map the data from Dao to Model
        IEnumerable<PrintDao>? Daos = JsonConvert.DeserializeObject<IEnumerable<PrintDao>>(JSON);
        if (Daos is null)
        {
            throw new JsonException("Could not deserialize Prints from the response.");
        }
        IEnumerable<Print> Prints = await Task.WhenAll
        (
            Daos.Select(async x => await PrintMapper.DaoToModel(x, Agent))
        );
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
        // convert the Model into Dao
        PrintDao Dao = PrintMapper.ModelToDao(Model);
        // convert the Dao into a JSON stream
        JsonContent Content = JsonContent.Create(Dao, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PostAsync
        (
            $"http://localhost:60000/Print",
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
        // convert the Model into Dao
        PrintDao Dao = PrintMapper.ModelToDao(NewModel);
        // convert the Dao into a JSON stream
        JsonContent Content = JsonContent.Create(Dao, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PutAsync
        (
            $"http://localhost:60000/Print/{TargetId}",
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