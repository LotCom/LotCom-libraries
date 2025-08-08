using LotCom.Types.Enums;

namespace LotCom.Types;

/// <summary>
/// Represents a Print operation in the LotCom database.
/// </summary>
public class Print(int Id, Process Process, Part Part, VariableFieldSet VariableFields, DateTime ProductionDate, PartialDataSet PrimaryDataSet, PartialDataSet? SecondaryDataSet = null, PartialDataSet? TertiaryDataSet = null)
{
    /// <summary>
    /// The Id of the Print object in the Database.
    /// </summary>
    public int Id = Id;
    
    /// <summary>
    /// The Process that printed the Label that the Print operation represents.
    /// </summary>
    public Process Process = Process;

    /// <summary>
    /// The Part attached to the Label that the Print operation represents.
    /// </summary>
    public Part Part = Part;

    /// <summary>
    /// The variable information attached to the Label that the Print operation represents.
    /// </summary>
    public VariableFieldSet VariableFields = VariableFields;

    /// <summary>
    /// The Date and Time at which the Label represented by the Print operation was printed.
    /// </summary>
    public DateTime ProductionDate = ProductionDate;

    /// <summary>
    /// Quantity, Shift, and Operator data of the Label that the Print operation represents.
    /// </summary>
    public PartialDataSet PrimaryDataSet = PrimaryDataSet;

    /// <summary>
    /// An additional partial set attached to the Label that the Print operation represents.
    /// </summary>
    public PartialDataSet? SecondaryDataSet = SecondaryDataSet;
    
    /// <summary>
    /// A third partial set attached to the Label that the Print operation represents.
    /// </summary>
    public PartialDataSet? TertiaryDataSet = TertiaryDataSet;

    /// <summary>
    /// Uses the Print's properties to construct and return a SerialNumber object.
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
            Part.Id,
            Literal
        );
    }
}