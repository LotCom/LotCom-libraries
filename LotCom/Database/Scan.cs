using System.Globalization;
using System.Net;
using LotCom.Enums;
using LotCom.Exceptions;
using LotCom.Extensions;
using LotCom.Types;

namespace LotCom.Database;

/// <summary>
/// A DatabaseEntry that represents a Scan operation in the LotCom database.
/// </summary>
public class Scan(Process Process, DateTime ScanTime, IPAddress ScanAddress, Part Part, PartialDataSet PrimaryDataSet, VariableFieldSet VariableFields, DateTime ProductionDate, PartialDataSet? FirstPartialDataSet = null, PartialDataSet? SecondPartialDataSet = null) : DatabaseEntry(Process, Part, PrimaryDataSet, VariableFields, ProductionDate, FirstPartialDataSet, SecondPartialDataSet)
{
    /// <summary>
    /// The Date and Time at which the Scan operation occurred. This is NOT the production time.
    /// </summary>
    public DateTime ScanTime = ScanTime;

    /// <summary>
    /// The IP Address of the Scanner producting the Scan operation.
    /// </summary>
    public IPAddress ScanAddress = ScanAddress;

    /// <summary>
    /// Converts the Scan into a database-ready CSV-formatted string.
    /// </summary>
    /// <returns></returns>
    public string ToCSV()
    {
        // combine partial basket data, if needed
        string FullQuantity = $"{PrimaryDataSet.Quantity.Value}";
        string FullShift = ShiftExtensions.ToString(PrimaryDataSet.Shift);
        string FullOperator = $"{PrimaryDataSet.Operator.Initials}";
        if (FirstPartialDataSet is not null)
        {
            FullQuantity += $":{FirstPartialDataSet.Quantity.Value}";
            FullShift += $":{ShiftExtensions.ToString(FirstPartialDataSet.Shift)}";
            FullOperator += $":{FirstPartialDataSet.Operator.Initials}";
        }
        if (SecondPartialDataSet is not null)
        {
            FullQuantity += $":{SecondPartialDataSet.Quantity.Value}";
            FullShift += $":{ShiftExtensions.ToString(SecondPartialDataSet.Shift)}";
            FullOperator += $":{SecondPartialDataSet.Operator.Initials}";
        }
        // return the full CSV line with combined Partial Data
        return $"{Process.FullName},{new Timestamp(ScanTime).Stamp},{ScanAddress},{Part.ToCSV()},{FullQuantity},{VariableFields.ToCSV()},{new Timestamp(ProductionDate).Stamp},{FullShift},{FullOperator}";
    }

    /// <summary>
    /// Parses an existing Scan entry (EntryString) from the database into a Scan object.
    /// </summary>
    /// <param name="EntryString"></param>
    /// <returns></returns>
    /// <exception cref="DatabaseException"></exception>
    /// <exception cref="FormatException"></exception>
    public static async Task<Scan> Parse(string EntryString)
    {
        // split the string to separate fields
        string[] SplitEntry = EntryString.Split(",");
        // open a new Database accessor
        ProcessData Db = new ProcessData();
        // asynchronously request the Process and Part from the database
        Process Process;
        Part Part;
        try
        {
            Process = await Db.GetIndividualProcessAsync(SplitEntry[0]);
            Part = await Db.GetProcessPartDataAsync(SplitEntry[0], SplitEntry[3]);
        }
        catch (SystemException _ex)
        {
            throw new DatabaseException($"Failed to get the requested Process '{SplitEntry[0]}' and/or Part '{SplitEntry[3]}'.", _ex);
        }
        // parse the Scan Time and the IP Address
        DateTime ScanTime = DateTime.ParseExact(SplitEntry[1], "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        IPAddress ScanAddress = IPAddress.Parse(SplitEntry[2]);
        // parse any PartialDataSets, including the primary set
        List<PartialDataSet?> Partials = PartialDataSet.Parse(SplitEntry[5], SplitEntry[^2], SplitEntry[^1])!;
        // ensure at least one PartialDataSet (primary) exists
        if (Partials.Count < 1 || Partials[0] is null)
        {
            throw new FormatException($"Could not parse the Primary Quantity, Shift, and Operator set from '{EntryString}'.");
        }
        // fill Partials to create a full set of Primary and two additional PartialDataSets
        while (Partials.Count < 3)
        {
            Partials.Add(null);
        }
        // parse the VariableFieldSet fields
        VariableFieldSet VariableFields = VariableFieldSet.ParseCSV(SplitEntry[6..^3], Process.RequiredFields);
        // parse the production date
        DateTime ProductionDate = DateTime.ParseExact(SplitEntry[^3], "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        // construct and return the Scan object
        return new Scan
        (
            Process: Process,
            ScanTime: ScanTime,
            ScanAddress: ScanAddress,
            Part: Part,
            PrimaryDataSet: Partials[0]!,
            VariableFields: VariableFields,
            ProductionDate: ProductionDate,
            FirstPartialDataSet: Partials[1],
            SecondPartialDataSet: Partials[2]
        );
    }

    /// <summary>
    /// Uses the Scan's properties to construct and return a SerialNumber object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public SerialNumber GetSerialNumber()
    {
        int Literal;
        // use the JBK number
        if (Process.Serialization == SerializationMode.JBK || Process.PassThroughType == PassThroughType.JBK)
        {
            Literal = VariableFields.JBKNumber!.Literal;
        }
        // use the Lot number
        else if (Process.Serialization == SerializationMode.Lot || Process.PassThroughType == PassThroughType.Lot)
        {
            Literal = VariableFields.LotNumber!.Literal;
        }
        // Process' Serialization is mis-configured
        else
        {
            throw new FormatException("There was a configuration issue with the Process. No Serialization is available.");
        }
        // construct and return a SerialNumber
        return new SerialNumber
        (
            Process.Serialization,
            Part,
            Literal
        );
    }
}