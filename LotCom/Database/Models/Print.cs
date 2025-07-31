namespace LotCom.Database.Models;

using LotCom.Types;

/// <summary>
/// Represents a Print record from the LotCom API.
/// </summary>
public class Print
{
    public Process Process { get; set; }

    public Part Part { get; set; }

    public VariableFieldSet VariableFields { get; set; }

    public PartialDataSet PrimaryData { get; set; }

    public PartialDataSet? SecondaryData { get; set; }

    public PartialDataSet? TertiaryData { get; set; }

    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Creates a new instance of the Print entity class.
    /// </summary>
    /// <param name="Process"></param>
    /// <param name="Part"></param>
    /// <param name="VariableFields"></param>
    /// <param name="PrimaryData"></param>
    /// <param name="SecondaryData"></param>
    /// <param name="TertiaryData"></param>
    /// <param name="ProductionDate"></param>
    public Print(Process Process, Part Part, VariableFieldSet VariableFields, PartialDataSet PrimaryData, PartialDataSet? SecondaryData, PartialDataSet? TertiaryData, DateTime ProductionDate)
    {
        this.Process = Process;
        this.Part = Part;
        this.VariableFields = VariableFields;
        this.PrimaryData = PrimaryData;
        this.SecondaryData = SecondaryData;
        this.TertiaryData = TertiaryData;
        this.ProductionDate = ProductionDate;
    }
}