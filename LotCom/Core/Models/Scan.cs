using System.Net;
using LotCom.Core.Enums;
using LotCom.Core.Types;

namespace LotCom.Core.Models;

/// <summary>
/// Represents a Scan operation in the LotCom database.
/// </summary>
public class Scan(int Id, Process ScanProcess, DateTime ScanDate, IPAddress ScanAddress, Process LabelProcess, Part Part, VariableFieldSet VariableFields, DateTime ProductionDate, PartialDataSet PrimaryData, PartialDataSet? SecondaryData = null, PartialDataSet? TertiaryData = null)
{
    /// <summary>
    /// The Id of the Scan object in the Database.
    /// </summary>
    public int Id = Id;

    /// <summary>
    /// The Process that produced the Scan operation.
    /// </summary>
    public Process ScanProcess = ScanProcess;

    /// <summary>
    /// The Date and Time at which the Scan operation occurred. This is NOT the production time.
    /// </summary>
    public DateTime ScanDate = ScanDate;

    /// <summary>
    /// The IP Address of the Scanner producting the Scan operation.
    /// </summary>
    public IPAddress ScanAddress = ScanAddress;

    /// <summary>
    /// The Process that printed the Label that the Scan operation represents.
    /// </summary>
    public Process LabelProcess = LabelProcess;

    /// <summary>
    /// The Part attached to the Label that the Scan operation represents.
    /// </summary>
    public Part Part = Part;

    /// <summary>
    /// The variable information attached to the Label that the Scan operation represents.
    /// </summary>
    public VariableFieldSet VariableFields = VariableFields;

    /// <summary>
    /// The Date and Time at which the Label represented by the Scan operation was printed.
    /// </summary>
    public DateTime ProductionDate = ProductionDate;

    /// <summary>
    /// Quantity, Shift, and Operator data of the Label that the Scan operation represents.
    /// </summary>
    public PartialDataSet PrimaryData = PrimaryData;

    /// <summary>
    /// An additional partial set attached to the Label that the Scan operation represents.
    /// </summary>
    public PartialDataSet? SecondaryData = SecondaryData;

    /// <summary>
    /// A third partial set attached to the Label that the Scan operation represents.
    /// </summary>
    public PartialDataSet? TertiaryData = TertiaryData;

    /// <summary>
    /// Uses the Scan's properties to construct and return a SerialNumber object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public SerialNumber GetSerialNumber()
    {
        int Literal;
        // use the JBK number
        if (LabelProcess.Serialization == SerializationMode.JBK || LabelProcess.PassThroughType == PassThroughType.JBK)
        {
            Literal = VariableFields.JBKNumber!.Literal;
        }
        // use the Lot number
        else if (LabelProcess.Serialization == SerializationMode.Lot || LabelProcess.PassThroughType == PassThroughType.Lot)
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
            LabelProcess.Serialization,
            Part.Id,
            Literal
        );
    }

    /// <summary>
    /// Compares two Scans as "identical" events, meaning the Serial, Part, and Date match.
    /// </summary>
    /// <param name="Comparison"></param>
    /// <returns></returns>
    public bool IsIdentical(Scan Comparison)
    {
        // make cheapest to most expensive comparisons
        if (LabelProcess.Id != Comparison.LabelProcess.Id)
        {
            return false;
        }
        else if (ProductionDate.CompareTo(Comparison.ProductionDate) != 0)
        {
            return false;
        }
        else if (!GetSerialNumber().Value.Equals(Comparison.GetSerialNumber().Value))
        {
            return false;
        }
        // all checks are equal; same scan
        return true;
    }

    /// <summary>
    /// Compares this Scan to Comparison and determines if Comparison is from a Process previous to this Scan's Process.
    /// </summary>
    /// <param name="Comparison"></param>
    /// <returns></returns>
    public bool IsFromPreviousProcess(Scan Previous)
    {
        // for Previous to be a previous Scan:
        // this Scan's PreviousProcesses has to contain Previous' Process Id
        if (!LabelProcess.HasPreviousProcess())
        {
            return false;
        }
        if (!LabelProcess.PreviousProcesses!.Contains(Previous.LabelProcess.Id))
        {
            return false;
        }
        // AND this Scan's SerialNumber must equal Previous' SerialNumber
        // OR this Scan's DeburrJBKNumber must equal Previous' SerialNumber 
        // -- Only if this is a Machining Process Scan
        string PreviousSerialNumber = Previous.GetSerialNumber().GetFormattedValue();
        if (LabelProcess.Type == ProcessType.Machining)
        {
            if (VariableFields.DeburrJBKNumber is null)
            {
                return false;
            }
            if (!VariableFields.DeburrJBKNumber.Formatted.Equals(PreviousSerialNumber))
            {
                return false;
            }
        }
        else
        {
            if (!GetSerialNumber().GetFormattedValue(LabelProcess).Equals(PreviousSerialNumber))
            {
                return false;
            }
        }
        // AND this Scan's Model Code must equal Previous' Model Code
        if (!Part.ModelNumber.Code.Equals(Previous.Part.ModelNumber.Code))
        {
            return false;
        }
        // AND Previous' Scan Date must be between 0 and 60 days before this Scan's Scan Date
        TimeSpan ElapsedTime = ScanDate.Subtract(Previous.ScanDate);
        if (ElapsedTime.Days < 0 || ElapsedTime.Days > 60)
        {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Checks if DateToCompare is within 0 and RangeInDays days before the Created Date of this Scan.
    /// </summary>
    /// <param name="RangeInDays"></param>
    /// <param name="DateToCompare"></param>
    /// <returns></returns>
    public bool CompareDateWithinRange(int RangeInDays, DateTime DateToCompare)
    {
        // subtract the comparison date from the Scan Date
        TimeSpan Difference = DateToCompare.Subtract(ScanDate);
        // check if the difference is between 0 and RangeInDays days
        if (Difference.Days > RangeInDays || Difference.Days < 0)
        {
            // either too old or newer than this Scan
            return false;
        }
        // Date to compare was between 0 and RangeInDays days in the past
        else
        {
            return true;
        }
    }
}