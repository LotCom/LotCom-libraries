using System.Net.Http.Headers;
using LotCom.DataAccess.Auth;
using LotCom.DataAccess.Extensions;

namespace LotCom.DataAccess;

/// <summary>
/// Provides controlled creation of HttpClient objects that meet the standards of the LotCom API.
/// </summary>
public static class HttpClientFactory
{
    /// <summary>
    /// Creates an HTTP request/response client that is configured for a LotCom system User Agent.
    /// </summary>
    public static HttpClient Create(UserAgent Agent)
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
        Client.DefaultRequestHeaders.Add
        (
            "User-Agent",
            $"LotCom"
            + $"{AppNameExtensions.ToString(Agent.AppName)}"
            + $"/{Agent.AppVersion}"
            + $" ({AppPlatformExtensions.ToString(Agent.Platform)}"
            + $"; {AppEnvironmentExtensions.ToString(Agent.Environment)})");
        return Client;
    }
}