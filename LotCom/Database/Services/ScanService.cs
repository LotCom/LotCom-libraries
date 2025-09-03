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
/// Provides access to the Scan Database.
/// </summary>
public static class ScanService
{
    /// <summary>
    /// Provides mapping methods.
    /// </summary>
    private static ScanMapper _mapper = new ScanMapper();

    /// <summary>
    /// Provides balancing for expensive processing.
    /// </summary>
    private static ScanBalancer _balancer = new ScanBalancer();

    /// <summary>
    /// Retrieves all Scans from the Database.
    /// </summary>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAll(HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scans");
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Scan");
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
        // convert the DTOs to Models
        IEnumerable<Scan> Scans = [];
        foreach (ScanDto _dto in Dtos)
        {
            Scans = Scans.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Scans;
    }
    
    /// <summary>
    /// Retrieves all Scans from the Database within a set range from the current date.
    /// </summary>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAllWithinRange(int WithinDaysOfCurrent, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scans within {WithinDaysOfCurrent} days");
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Scan/within?days={WithinDaysOfCurrent}");
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
        // convert the DTOs to Models
        IEnumerable<Scan> Scans = [];
        foreach (ScanDto _dto in Dtos)
        {
            Scans = Scans.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Scans;
    }

    /// <summary>
    /// Retrieves a single Scan from the Database using its Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<Scan?> Get(int id, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scan {id}");
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Scan/{id}");
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
        return await _mapper.DtoToModel(Dto, Client, Agent);
    }

    /// <summary>
    /// Retrieves all Scans produced by ProcessId from the Database.
    /// </summary>
    /// <param name="ProcessId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetAllForProcess(int ProcessId, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scans by {ProcessId}");
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"https://lotcom.yna.us/api/Scan/by?" +
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
        // convert the DTOs to Models
        IEnumerable<Scan> Scans = [];
        foreach (ScanDto _dto in Dtos)
        {
            Scans = Scans.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
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
    public static async Task<IEnumerable<Scan>?> GetOnDateByProcess(DateTime Date, int ProcessId, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scans on {Date.Date} by {ProcessId}");
        HttpResponseMessage? Response = await Client.GetAsync
        (
            $"https://lotcom.yna.us/api/Scan/onDateBy?" +
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
        // convert the DTOs to Models
        IEnumerable<Scan> Scans = [];
        foreach (ScanDto _dto in Dtos)
        {
            Scans = Scans.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Scans;
    }

    /// <summary>
    /// Retrieves all Scans that contain serialNumber in their Serial Number (JBK/Lot) field.
    /// </summary>
    /// <param name="serialNumber"></param>
    /// <param name="Client"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<IEnumerable<Scan>?> GetWithSerialNumber(object serialNumber, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API GET Scans with serial {serialNumber}");
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
            $"https://lotcom.yna.us/api/Scan/serialNumber?serialNumber={Serial}"
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
        // convert the DTOs to Models
        IEnumerable<Scan> Scans = [];
        foreach (ScanDto _dto in Dtos)
        {
            Scans = Scans.Append(await _mapper.DtoToModel(_dto, Client, Agent));
        }
        return Scans;
    }

    /// <summary>
    /// Sends a Scan to the Database to be inserted.
    /// </summary>
    /// <param name="Model"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> Create(Scan Model, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API POST Scan");
        // convert the Model into Dto
        ScanDto Dto = _mapper.ModelToDto(Model);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PostAsync
        (
            $"https://lotcom.yna.us/api/Scan",
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
    public static async Task<bool> Update(int TargetId, Scan NewModel, HttpClient Client, UserAgent Agent)
    {
        Console.WriteLine($"API PUT Scan {TargetId}");
        // convert the Model into Dto
        ScanDto Dto = _mapper.ModelToDto(NewModel);
        // convert the Dto into a JSON stream
        JsonContent Content = JsonContent.Create(Dto, new MediaTypeHeaderValue("application/json"));
        // send the PUT request
        HttpResponseMessage Response = await Client.PutAsync
        (
            $"https://lotcom.yna.us/api/Scan/{TargetId}",
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