using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// An identifier for Dies used to cast Parts in a Basket. Follows a strict one- or two-digit format.
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
    private const int MaxValue = 50;

    /// <summary>
    /// The raw literal value of the Die Number.
    /// </summary>
    [ObservableProperty]
    public partial int Literal { get; set; }

    /// <summary>
    /// Confirms that Value is a valid value for this datatype.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    private static bool IsValidValue(int Value)
    {
        return Value <= MaxValue && Value >= MinValue;
    }

    /// <summary>
    /// Creates a new DieNumber from Value.
    /// </summary>
    /// <param name="Value"></param>
    public DieNumber(int Value)
    {
        // confirm that Value falls within the allowed literal range
        if (!IsValidValue(Value))
        {
            throw new ArgumentException($"'{Value}' is outside the allowed range of the DieNumber class.", nameof(Value));
        }
        Literal = Value;
    }
}