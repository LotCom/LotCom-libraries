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
    public static async Task<Print?> Get(int id, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Print {id}");
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
        return await _mapper.DtoToModel(Dto, Client, Agent);
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
    public static async Task<IEnumerable<Print>?> GetOnDateByProcess(DateTime Date, int ProcessId, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Prints on {Date.Date} by {ProcessId}");
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
        // convert the DTOs to Models
        IEnumerable<Print> Prints = [];
        foreach (PrintDto _dto in Dtos)
        {
            Prints = Prints.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Prints;
    }

    /// <summary>
    /// Retrieves all Prints that contain serialNumber in their Serial Number (JBK/Lot) field.
    /// </summary>
    /// <param name="serialNumber"></param>
    /// <param name="Client"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Print>?> GetWithSerialNumber(object serialNumber, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Prints with serial {serialNumber}");
        int Serial;
        if (serialNumber.GetType().Equals(typeof(int)))
        {
            Serial = (int)serialNumber;
        }
        else if (serialNumber.GetType().Equals(typeof(string)))
        {
            Serial = int.Parse((string)serialNumber);
        }
        else
        {
            throw new ArgumentException($"Cannot query for a Serial Number using an object of type {serialNumber.GetType()}", nameof(serialNumber));
        }
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"https://lotcom.yna.us/api/Print/serialNumber?serialNumber={Serial}"
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
        // convert the DTOs to Models
        IEnumerable<Print> Prints = [];
        foreach (PrintDto _dto in Dtos)
        {
            Prints = Prints.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Prints;
    }

    /// <summary>
    /// Sends a Print to the Database to be inserted.
    /// </summary>
    /// <param name="Model"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Create(Print Model, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API POST Print");
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
    public static async Task<bool> Update(int TargetId, Print NewModel, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API PUT Print {TargetId}");
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