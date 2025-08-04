using LotCom.Enums;
using LotCom.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotCom.Types;

/// <summary>
/// Create a new Serial Number for Part using SerializationMode.
/// </summary>
/// <param name="Mode">The mode of Serialization this Serial Number uses.</param>
/// <param name="Part">The Part this Serial Number was assigned to.</param>
/// <param name="Value">The Value to attempt to apply to this Serial Number.</param>
public class SerialNumber(SerializationMode Mode, int PartId, int Value)
{
    /// <summary>
    /// The mode of Serialization that the Serial Number uses.
    /// </summary>
    public SerializationMode Mode {get;} = Mode;

    /// <summary>
    /// The Id of the Part the Serial Number has been assigned to.
    /// </summary>
    public int PartId {get;} = PartId;

    /// <summary>
    /// The Serial Number's literal value.
    /// </summary>
    public int Value {get; private set;} = Value;

    /// <summary>
    /// Returns the SerialNumber's value as a string with leading zeroes to enforce formatting.
    /// </summary>
    /// <returns></returns>
    public string GetFormattedValue()
    {
        // enforce leading zero-padding format
        string FormattedNumber = Value.ToString();
        if (Mode == SerializationMode.None)
        {
            return FormattedNumber;
        }
        else if (Mode == SerializationMode.JBK)
        {
            // enforce 3-length format
            while (FormattedNumber.Length < 3)
            {
                FormattedNumber = $"0{FormattedNumber}";
            }
        }
        else
        {
            // enforce 9-length format
            while (FormattedNumber.Length < 9)
            {
                FormattedNumber = $"0{FormattedNumber}";
            }
        }
        return FormattedNumber;
    }

    /// <summary>
    /// Formats the Serial Number as a JSON string that can be written to a File and parsed as JSON text.
    /// </summary>
    /// <returns></returns>
    public string ToJSON()
    {
        return
            "{" +
                $"\"Mode\":\"{Mode}\"," +
                $"\"Part\":\"{PartId}\"," +
                $"\"Value\":\"{Value}\"" +
            "}";
    }

    /// <summary>
    /// Attempts to parse a SerialNumber object from a JSON formatted string.
    /// </summary>
    /// <remarks>
    /// Throws JsonException if there were any errors parsing any part of the Serial Number from Line.
    /// Throws ArgumentException if the parsed Part Number was not defined for the parsed Process.
    /// </remarks>
    /// <param name="Line"></param>
    /// <returns>A SerialNumber object.</returns>
    /// <exception cref="JsonException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static SerialNumber ParseJSON(string Line)
    {
        SerializationMode Mode;
        int PartId;
        int Value;
        // parse Line into JSON
        JObject JSON = JObject.Parse(Line);
        JToken? RawPart = JSON["Part"];
        JToken? RawValue = JSON["Value"];
        try
        {
            Mode = SerializationModeExtensions.FromString(JSON["Mode"]!.ToString());
        }
        catch (ArgumentException)
        {
            throw new JsonException($"No SerializationMode found in cached Serial Number '{Line}'.");
        }
        // confirm the Part key has a valid value and convert it to a Part
        if (RawPart is null)
        {
            throw new JsonException($"No Part found in cached Serial Number '{Line}'.");
        }
        try
        {
            PartId = int.Parse(RawPart["Part"]!.ToString());
        }
        catch
        {
            throw new JsonException($"Could not parse a Part Id from cached Serial Number '{Line}'.");
        }
        // confirm the Value key has a valid value and that it is of type int
        if (RawValue is null) 
        {
            throw new JsonException($"No Value found in cached Serial Number '{Line}'.");
        }
        Value = int.Parse(RawValue.ToString());
        return new SerialNumber(Mode, PartId, Value);
    }
}