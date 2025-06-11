using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

public partial class Quantity : ObservableObject
{
    /// <summary>
    /// The number of items represented by this Quantity object.
    /// </summary>
    [ObservableProperty]
    public partial int Value { get; set; }

    /// <summary>
    /// Creates a new Quantity to represent a number of items.
    /// </summary>
    /// <param name="Value">The count of items this Quantity should represent.</param>
    /// <exception cref="ArgumentException"></exception>
    public Quantity(int Value)
    {
        // ensure that the Quantity is at least 0 (non-negative)
        if (Value < 0)
        {
            throw new ArgumentException("Cannot instantiate a Quantity that represents less than 0 items.", nameof(Value));
        }
        this.Value = Value;
    }

    /// <summary>
    /// Confirms that the Quantity object represents AT LEAST 1 item and is not at the default 0 value.
    /// </summary>
    /// <returns></returns>
    public bool ConfirmPositiveCount()
    {
        return Value > 0;
    }

    /// <summary>
    /// Converts the object into a string. Uses the Quantity's Value value as the source for this string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Value.ToString();
    }
}