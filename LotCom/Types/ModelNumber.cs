using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// An identifying code for Part Models that follows a strict three- or four-character, alphanumerical, and uppercase format.
/// </summary>
public partial class ModelNumber : ObservableObject
{
    /// <summary>
    /// The code value of the Model Number.
    /// </summary>
    [ObservableProperty]
    public partial string Code { get; set; }

    /// <summary>
    /// Confirms that Code is a valid value for this datatype.
    /// </summary>
    /// <param name="Code"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <returns></returns>
    private static bool IsValidValue(string Code)
    {
        return ModelRegex().IsMatch(Code);
    }

    /// <summary>
    /// Creates a new ModelNumber from Value.
    /// </summary>
    /// <param name="Code"></param>
    public ModelNumber(string Code)
    {
        // confirm that Value falls within the allowed code length
        if (!IsValidValue(Code))
        {
            throw new ArgumentException($"'{Code}' is outside the allowed length of codes for the ModelNumber class.", nameof(Code));
        }
        this.Code = Code.ToUpper();
    }

    /// <summary>
    /// Converts the object into a string. Uses the ModelNumber's Code value as the source for this string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Code;
    }

    // COMPILED REGEX PATTERNS 

    [GeneratedRegex(@"^[a-zA-Z0-9][a-zA-Z0-9][a-zA-Z0-9][a-zA-Z0-9]?$")]
    private static partial Regex ModelRegex();
}