using LotCom.Core.Enums;
using LotCom.Core.Types;
using LotCom.Database.Auth;
using Newtonsoft.Json;

namespace LotCom.Database.Services;

public static class SerialFeedService
{
    /// <summary>
    /// Consumes and returns the next JBK Number in the feed for PartId.
    /// </summary>
    /// <param name="PartId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<SerialNumber> ConsumeJBKNumber(int PartId, HttpClient Client, UserAgent Agent)
    {
        // configure and execute the API call
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Serial/consumeJBKFor?partId={PartId}");
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
        // deserialize the JSON response
        int? Number = JsonConvert.DeserializeObject<int>(JSON);
        if (Number is null)
        {
            throw new JsonException("Could not deserialize an int from the response.");
        }
        return new SerialNumber(SerializationMode.JBK, PartId, (int)Number);
    }
    
    /// <summary>
    /// Consumes and returns the next Lot Number in the feed for PartId.
    /// </summary>
    /// <param name="PartId"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public static async Task<SerialNumber> ConsumeLotNumber(int PartId, HttpClient Client, UserAgent Agent)
    {
        // configure and execute the API call
        HttpResponseMessage? Response = await Client.GetAsync($"https://lotcom.yna.us/api/Serialize/consumeLotFor?partId={PartId}");
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
        // deserialize the JSON response
        int? Number = JsonConvert.DeserializeObject<int>(JSON);
        if (Number is null)
        {
            throw new JsonException("Could not deserialize an int from the response.");
        }
        return new SerialNumber(SerializationMode.JBK, PartId, (int)Number);
    }
}