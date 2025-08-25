using LotCom.Core.Enums;

namespace LotCom.Core.Extensions;

public static class PassThroughTypeExtensions
{
    /// <summary>
    /// Converts a PassThroughType to a string.
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    public static string ToString(this PassThroughType Type)
    {
        if (Type == PassThroughType.JBK)
        {
            return "JBK";
        }
        else if (Type == PassThroughType.Lot)
        {
            return "Lot";
        }
        else
        {
            return "None";
        }
    }

    /// <summary>
    /// Attempts to convert a string literal to an PassThroughType enum value.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static PassThroughType FromString(this string String)
    {
        if (String is null || String.Equals(""))
        {
            return PassThroughType.None;
        }
        else if (String.Equals("JBK"))
        {
            return PassThroughType.JBK;
        }
        else if (String.Equals("Lot"))
        {
            return PassThroughType.Lot;
        }
        else
        {
            throw new ArgumentException($"Cannot convert {String} to a PassThroughType.");
        }
    }
}