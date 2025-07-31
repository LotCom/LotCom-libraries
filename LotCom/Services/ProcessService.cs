using System.Net.Http.Headers;

namespace LotCom.Services;

public static class ProcessService
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

    public static async Task RequestProcesses()
    {
        HttpClient Client = ConfigureHttp();
        string? Response = await Client.GetStringAsync("http://localhost:5196/Process/1");
        if (Response is null)
        {
            Console.WriteLine("API had no response.");
        }
        else
        {
            Console.WriteLine(Response);
        }
    }
}