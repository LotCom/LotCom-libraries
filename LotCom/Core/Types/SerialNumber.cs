using LotCom.Core.Enums;
using LotCom.Core.Extensions;
using LotCom.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotCom.Core.Types;

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
    /// <remarks>
    /// NOTE that this method does not account for Pass-through Serial Numbers. 
    /// Use the overload GetFormattedValue(ParentProcess) to format these numbers.
    /// </remarks>
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
    /// Uses the passed Process' Serialization configuration to format the SerialNumber's value as a string with leading zeroes.
    /// </summary>
    /// <param name="ParentProcess"></param>
    /// <returns></returns>
    public string GetFormattedValue(Process ParentProcess)
    {
        // enforce leading zero-padding format
        int FormattingMode = -1; // 0 for JBK, 1 for Lot
        string FormattedNumber = Value.ToString();
        if
        (
            ParentProcess.Serialization == SerializationMode.None
            && ParentProcess.PassThroughType == PassThroughType.None
        )
        {
            // No serialization format is provided; return raw number
            return FormattedNumber;
        }
        if
        (
            ParentProcess.Serialization == SerializationMode.JBK
            || ParentProcess.PassThroughType == PassThroughType.JBK
        )
        {
            // format number using JBK format
            FormattingMode = 0;
        }
        else if
        (
            ParentProcess.Serialization == SerializationMode.Lot
            || ParentProcess.PassThroughType == PassThroughType.Lot
        )
        {
            FormattingMode = 1;
        }
        // always pad to at least three digits
        while (FormattedNumber.Length < 3)
        {
            FormattedNumber = $"0{FormattedNumber}";
        }
        if (FormattingMode != 1)
        {
            // JBK mode; number is formatted
            return FormattedNumber;
        }
        else
        {
            // Lot mode; pad further to nine digits
            while (FormattedNumber.Length < 9)
            {
                FormattedNumber = $"0{FormattedNumber}";
            }
            return FormattedNumber;
        }
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
            PartId = int.Parse(RawPart.ToString());
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