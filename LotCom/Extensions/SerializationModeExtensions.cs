using LotCom.Enums;

namespace LotCom.Extensions;

/// <summary>
/// Provides extension methods on the SerializationMode enum.
/// </summary>
public static class SerializationModeExtensions
{
    /// <summary>
    /// Converts a SerializationMode to a string.
    /// </summary>
    /// <returns></returns>
    public static string ToString(this SerializationMode Mode)
    {
        if (Mode == SerializationMode.JBK)
        {
            return "JBK";
        }
        else if (Mode == SerializationMode.Lot)
        {
            return "Lot";
        }
        else
        {
            return "None";
        }
    }

    /// <summary>
    /// Attempts to convert a string literal to a SerializationMode enum value.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static SerializationMode FromString(this string String)
    {
        if (String is null || String.Equals("") || String.Equals("None"))
        {
            return SerializationMode.None;
        }
        else if (String.Equals("JBK"))
        {
            return SerializationMode.JBK;
        }
        else if (String.Equals("Lot"))
        {
            return SerializationMode.Lot;
        }
        else
        {
            throw new ArgumentException($"Cannot convert {String} to a SerializationMode.");
        }
    }
}