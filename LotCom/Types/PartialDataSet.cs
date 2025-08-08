using CommunityToolkit.Mvvm.ComponentModel;
using LotCom.Types.Enums;
using LotCom.Types.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotCom.Types;

/// <summary>
/// Provides a structure to capture Data related to a Partially-completed Basket.
/// </summary>
/// <param name="Quantity">The number of Parts produced during the Shift captured by the PartialDataSet.</param>
/// <param name="Shift">The Shift Number captured by the PartialDataSet.</param>
/// <param name="Operator">The Operator who created the PartialDataSet.</param>
public partial class PartialDataSet(Quantity Quantity, Shift Shift, Operator Operator) : ObservableObject()
{
    /// <summary>
    /// The number of Parts produced during the Shift captured by the PartialDataSet.
    /// </summary>
    [ObservableProperty]
    public partial Quantity Quantity { get; set; } = Quantity;

    /// <summary>
    /// The Shift Number captured by the PartialDataSet.
    /// </summary>
    [ObservableProperty]
    public partial Shift Shift { get; set; } = Shift;

    /// <summary>
    /// The Operator who created the PartialDataSet.
    /// </summary>
    [ObservableProperty]
    public partial Operator Operator { get; set; } = Operator;

    /// <summary>
    /// Attempts to parse and construct a PartialDataSet object from a JSON stream.
    /// </summary>
    /// <param name="JSON"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static PartialDataSet ParseJSON(JToken JSON)
    {
        // attempt to parse a Quantity, Shift Number, and Operator from the passed JSON stream
        Quantity Quantity;
        Shift Shift;
        Operator Operator;
        try
        {
            Quantity = new Quantity(int.Parse(JSON["Quantity"]!.ToString()));
        }
        catch
        {
            throw new JsonException($"Could not parse a Quantity value from {JSON["FirstPartialDataSet"]!["Quantity"]!}.");
        }
        try
        {
            Shift = ShiftExtensions.FromString(JSON!["Shift"]!.ToString());
        }
        catch
        {
            throw new JsonException($"Could not parse a Quantity value from {JSON["FirstPartialDataSet"]!["Shift"]!}.");
        }
        try
        {
            Operator = new Operator(JSON["Operator"]!.ToString());
        }
        catch
        {
            throw new JsonException($"Could not parse a Quantity value from {JSON["FirstPartialDataSet"]!["Operator"]!}.");
        }
        return new PartialDataSet(Quantity, Shift, Operator);
    }

    /// <summary>
    /// Recursively parses 1 to 3 PartialDataSets from Quantity, Shift, and Operator.
    /// </summary>
    /// <remarks>
    /// Expected argument formats:
    /// ["123", "1", "ABC"] (Base, single parse) OR 
    /// ["123:456:789", "1:2:3", "ABC:DEF:GHI"] (Recursive, 2-3 parses).
    /// </remarks>
    /// <param name="Quantity">A string of digits or sets of digits separated by colons.</param>
    /// <param name="Shift">A string of a single digit or a set of single digits separated by colons.</param>
    /// <param name="Operator">A string of letters or sets of letters separated by colons.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<PartialDataSet> Parse(string Quantity, string Shift, string Operator)
    {
        List<PartialDataSet> ParsedSets = [];
        // RECURSIVE CASE: colon in quantity field indicates the existence of multiple PartialDataSets
        if (Quantity.Contains(':'))
        {
            // split passed fields into lists
            string[] SplitQuantity = Quantity.Split(":");
            string[] SplitShift = Shift.Split(":");
            string[] SplitOperator = Operator.Split(":");
            // for loop over 2 or 3 possible PartialDataSet
            for (int i = 0; i < SplitQuantity.Length; i++)
            {
                // parse and add parsed PartialDataSets to the return list
                try
                {
                    ParsedSets.Add(Parse(SplitQuantity[0], SplitShift[0], SplitOperator[0])[0]);
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }
        // BASE CASE: there is a single PartialDataSet present to parse from the CSV fields
        else
        {
            try
            {
                // construct a single PartialDataSet from the passed field values
                ParsedSets.Add
                (
                    new PartialDataSet
                    (
                        new Quantity(int.Parse(Quantity)),
                        ShiftExtensions.FromString(Shift),
                        new Operator(Operator)
                    )
                );
            }
            // the int.Parse() call faulted
            catch (FormatException)
            {
                throw new ArgumentException($"Could not parse int Quantity from '{Quantity}'.");
            }
            // one of the three Quantity, Shift, Operator parses faulted
            catch (ArgumentException)
            {
                throw new ArgumentException($"Could not parse a PartialDataSet from '{Quantity}, {Shift}, {Operator}'.");
            }
        }
        // return up to 3 PartialDataSet objects
        return ParsedSets;
    }

    /// <summary>
    /// Validates that the DataSet's Quantity is greater than 0.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void SelfValidate()
    {
        // validate quantity, operator
        if (!Quantity.ConfirmPositiveCount())
        {
            throw new ArgumentException("Please enter a valid Production Quantity before printing a Label.");
        }
        if (!Operator.ConfirmProperInitials())
        {
            throw new ArgumentException("Please enter valid Operator Initials before printing a Label.");
        }
    }
}