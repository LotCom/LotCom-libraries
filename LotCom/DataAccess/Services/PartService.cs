using System.Net.Http.Headers;
using LotCom.DataAccess.Mappers;
using LotCom.DataAccess.Models;
using LotCom.Types;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Services;

public static class PartService
{
    /// <summary>
    /// Configures an HTTP request/response client for this application.
    /// </summary>
    private static HttpClient ConfigureHttp()
    {
        HttpClient Client = new HttpClient();
        // cleans the HttpClient's accepted response header configuration
        Client.DefaultRequestHeaders.Accept.Clear();
        // adds the default API response header to the accepted response configuration
        Client.DefaultRequestHeaders.Accept.Add
        (
            new MediaTypeWithQualityHeaderValue
            (
                "application/json"
            )
        );
        // adds the default UA header for custom apps (ex. LotCom Printer 1.0.0)
        Client.DefaultRequestHeaders.Add("User-Agent", "LotComPrinter/1.0.0 (Windows; .NET)");
        return Client;
    }

    public static async Task<Part?> Get(int id)
    {
        HttpClient Client = ConfigureHttp();
        string? Response = await Client.GetStringAsync($"http://localhost:60000/Part/{id}");
        if (Response is null)
        {
            return null;
        }
        PartDto? Dto = JsonConvert.DeserializeObject<PartDto>(Response);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Part from the response.");
        }
        return await PartMapper.DtoToModel(Dto);
    }


    public static async Task<List<Part>?> GetPrintedByProcess(int id)
    {
        HttpClient Client = ConfigureHttp();
        string? Response = await Client.GetStringAsync($"http://localhost:60000/Part/{id}");
        if (Response is null)
        {
            return null;
        }
        PartDto? Dto = JsonConvert.DeserializeObject<PartDto>(Response);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Part from the response.");
        }
        return [await PartMapper.DtoToModel(Dto)];
    }


    public static async Task<List<Part>?> GetScannedByProcess(int id)
    {
        HttpClient Client = ConfigureHttp();
        string? Response = await Client.GetStringAsync($"http://localhost:60000/Part/{id}");
        if (Response is null)
        {
            return null;
        }
        PartDto? Dto = JsonConvert.DeserializeObject<PartDto>(Response);
        if (Dto is null)
        {
            throw new JsonException("Could not deserialize a Part from the response.");
        }
        return [await PartMapper.DtoToModel(Dto)];
    }
}