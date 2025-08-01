using System.Net.Http.Headers;
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

    public static async Task<IEnumerable<Print>?> GetAllPrints()
    {
        HttpClient Client = ConfigureHttp();
        string? Response = await Client.GetStringAsync("http://localhost:60000/Prints");
        if (Response is null)
        {
            Console.WriteLine("API had no response.");
            return null;
        }
        IEnumerable<PrintDto>? Prints = JsonConvert.DeserializeObject<IEnumerable<PrintDto>>(Response);
        if (Prints is null)
        {
            throw new JsonException("Could not deserialize the Prints from the response");
        }
        IEnumerable<Task<Print>> MapTasks = Prints.Select(PrintMapper.DtoToModel);
        Print[] Models = await Task.WhenAll(MapTasks);
        if (Models is null)
        {
            return [];
        }
        Console.WriteLine(Models.Length);
        return Models;
    }
}