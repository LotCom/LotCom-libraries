using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// An identifier for Dies used to cast Parts in a Basket. 
/// Follows a strict one- or two-digit format with an optional "A" or "B" suffix for split Dies.
/// </summary>
public partial class DieNumber : ObservableObject
{
    /// <summary>
    /// The absolute lowest digit literal that can be assigned to a Die Number.
    /// </summary>
    private const int MinValue = 1;

    /// <summary>
    /// The absolute highest digit literal that can be assigned to a Die Number.
    /// </summary>
    private const int MaxValue = 99;

    /// <summary>
    /// The raw literal value of the Die Number.
    /// </summary>
    [ObservableProperty]
    public partial int Literal { get; set; }

    /// <summary>
    /// An optional "A" or "B" indicator for split Dies.
    /// </summary>
    [ObservableProperty]
    public partial char? SplitIdentifier { get; set; }

    /// <summary>
    /// Confirms that Value is a valid value for this datatype.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    private static bool IsValidValue(string Value)
    {
        // is value too long?
        if (Value.Length > 3)
        {
            return false;
        }
        // does value contain an invalid A/B indicator?
        if (AlphaRegex().IsMatch(Value))
        {
            if (!Value.Contains('A') && !Value.Contains('B'))
            {
                return false;
            }
        }
        // is value's integer segment valid?
        int IntSegment = int.Parse(Value.Replace("A", "").Replace("B", ""));
        return IntSegment <= MaxValue && IntSegment >= MinValue;
    }

    /// <summary>
    /// Creates a new DieNumber from Value.
    /// </summary>
    /// <param name="Value"></param>
    public DieNumber(string Value)
    {
        Value = Value.ToUpper();
        // confirm that Value falls within the allowed range
        if (!IsValidValue(Value))
        {
            throw new ArgumentException($"'{Value}' is outside the allowed range of the DieNumber class.", nameof(Value));
        }
        // assign the literal int value
        Literal = int.Parse(Value.Replace("A", "").Replace("B", ""));
        // assign the optional split identifier
        if (Value.Contains('A'))
        {
            SplitIdentifier = 'A';
        }
        else if (Value.Contains('B'))
        {
            SplitIdentifier = 'A';
        }
        else
        {
            SplitIdentifier = null;
        }
    }

    /// <summary>
    /// Converts the object into a string. 
    /// Uses the DieNumber's Literal value as the source for this string.
    /// Additionally appends a SplitIdentifier if added to the object.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string Full = Literal.ToString();
        if (SplitIdentifier is not null)
        {
            Full += SplitIdentifier;
        }
        return Full;
    }
    
    // Compiled Regexs

    [GeneratedRegex(@"[A-Za-z]")]
    private static partial Regex AlphaRegex();
}