using LotCom.Core.Enums;

namespace LotCom.Core.Extensions;

/// <summary>
/// Provides extension methods on the ProcessType enum.
/// </summary>
public static class ProcessTypeExtensions
{
    /// <summary>
    /// Converts a ProcessType to a string.
    /// </summary>
    /// <returns></returns>
    public static string ToString(this ProcessType Type)
    {
        if (Type == ProcessType.Casting)
        {
            return "Casting";
        }
        else if (Type == ProcessType.Deburring)
        {
            return "Deburring";
        }
        else if (Type == ProcessType.ShotBlasting)
        {
            return "Shot-Blasting";
        }
        else if (Type == ProcessType.Machining)
        {
            return "Machining";
        }
        else if (Type == ProcessType.Processing)
        {
            return "Processing";
        }
        else if (Type == ProcessType.Welding)
        {
            return "Welding";
        }
        else if (Type == ProcessType.Clinching)
        {
            return "Clinching";
        }
        else if (Type == ProcessType.Comping)
        {
            return "Comping";
        }
        else if (Type == ProcessType.SubAssembly)
        {
            return "Sub-Assembly";
        }
        else
        {
            return "Assembly";
        }
    }

    /// <summary>
    /// Attempts to convert a string literal to a ProcessType enum value.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ProcessType FromString(this string String)
    {
        if (String.Equals("Casting"))
        {
            return ProcessType.Casting;
        }
        if (String.Equals("Deburring"))
        {
            return ProcessType.Deburring;
        }
        if (String.Equals("Shot-Blasting"))
        {
            return ProcessType.ShotBlasting;
        }
        if (String.Equals("Machining"))
        {
            return ProcessType.Machining;
        }
        if (String.Equals("Processing"))
        {
            return ProcessType.Processing;
        }
        if (String.Equals("Welding"))
        {
            return ProcessType.Welding;
        }
        if (String.Equals("Clinching"))
        {
            return ProcessType.Clinching;
        }
        if (String.Equals("Comping"))
        {
            return ProcessType.Comping;
        }
        if (String.Equals("Sub-Assembly"))
        {
            return ProcessType.SubAssembly;
        }
        if (String.Equals("Assembly"))
        {
            return ProcessType.Assembly;
        }
        else
        {
            throw new ArgumentException($"Cannot convert {String} to a ProcessType.");
        }
    }
}