namespace LotCom.DataAccess.Models;

/// <summary>
/// Defines the publicly-available entity structure of a Print.
/// </summary>
public class PrintDto(int ProcessId, int PartId, int Quantity, int? SecondaryQuantity, int? TertiaryQuantity, int Shift, int? SecondaryShift, int? TertiaryShift, string Operator, string? SecondaryOperator, string? TertiaryOperator, int? JBKNumber, string? LotNumber, string? DieNumber, int? DeburrJBKNumber, string? HeatNumber, string ProductionDate)
{
    public int Id { get; set; }

    public int ProcessId { get; set; } = ProcessId;

    public int PartId { get; set; } = PartId;

    public int Quantity { get; set; } = Quantity;

    public int? SecondaryQuantity { get; set; } = SecondaryQuantity;

    public int? TertiaryQuantity { get; set; } = TertiaryQuantity;

    public int Shift { get; set; } = Shift;

    public int? SecondaryShift { get; set; } = SecondaryShift;

    public int? TertiaryShift { get; set; } = TertiaryShift;

    public string Operator { get; set; } = Operator;

    public string? SecondaryOperator { get; set; } = SecondaryOperator;

    public string? TertiaryOperator { get; set; } = TertiaryOperator;

    public int? JBKNumber { get; set; } = JBKNumber;

    public string? LotNumber { get; set; } = LotNumber;

    public string? DieNumber { get; set; } = DieNumber;

    public int? DeburrJBKNumber { get; set; } = DeburrJBKNumber;

    public string? HeatNumber { get; set; } = HeatNumber;

    public string ProductionDate { get; set; } = ProductionDate;
}