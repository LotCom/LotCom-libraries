using LotCom.Types;

namespace LotCom.Database;

/// <summary>
/// Base class for defining existing Database entries in the LotCom database.
/// </summary>
public class DatabaseEntry(Process Process, Part Part, PartialDataSet PrimaryDataSet, VariableFieldSet VariableFields, DateTime ProductionDate, PartialDataSet? FirstPartialDataSet = null, PartialDataSet? SecondPartialDataSet = null)
{
    /// <summary>
    /// The Process that created the Entry.
    /// </summary>
    public Process Process = Process;

    /// <summary>
    /// The Part that the Entry is linked to.
    /// </summary>
    public Part Part = Part;

    /// <summary>
    /// The primary Quantity, Shift, and Operator of the Entry's basket.
    /// </summary>
    public PartialDataSet PrimaryDataSet = PrimaryDataSet;

    /// <summary>
    /// The manufacturer tracing information of the Entry's basket.
    /// </summary>
    public VariableFieldSet VariableFields = VariableFields;

    /// <summary>
    /// The Date and Time at which the Entry's basket was produced.
    /// </summary>
    public DateTime ProductionDate = ProductionDate;

    /// <summary>
    /// An optional PartialDataSet that represents Shift split production data for the Entry's basket.
    /// </summary>
    public PartialDataSet? FirstPartialDataSet = FirstPartialDataSet;

    /// <summary>
    /// An optional second PartialDataSet that represents Shift split production data for the Entry's basket.
    /// </summary>
    public PartialDataSet? SecondPartialDataSet = SecondPartialDataSet;
}