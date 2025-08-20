using System.ComponentModel.DataAnnotations;

namespace LotCom.DataAccess.Entities;

/// <summary>
/// Defines the Database entity structure of a Print.
/// </summary>
public class PrintEntity(int ProcessId, int PartId, int Quantity, int? SecondaryQuantity, int? TertiaryQuantity, int Shift, int? SecondaryShift, int? TertiaryShift, string Operator, string? SecondaryOperator, string? TertiaryOperator, int? JBKNumber, string? LotNumber, string? DieNumber, int? DeburrJBKNumber, string? HeatNumber, string ProductionDate)
{
    // protected properties (not transfered by DTO Layer)
    [MaxLength(20)]
    public string Created { get; set; } = "";

    [MaxLength(20)]
    public string Updated { get; set; } = "";

    // open properties (transfered by DTO Layer)
    [Key]
    public int Id { get; set; } = 0;
    
    [Required]
    public int ProcessId { get; set; } = ProcessId;

    [Required]
    public int PartId { get; set; } = PartId;

    [Required]
    public int Quantity { get; set; } = Quantity;

    public int? SecondaryQuantity { get; set; } = SecondaryQuantity;

    public int? TertiaryQuantity { get; set; } = TertiaryQuantity;

    [Required]
    public int Shift { get; set; } = Shift;

    public int? SecondaryShift { get; set; } = SecondaryShift;

    public int? TertiaryShift { get; set; } = TertiaryShift;

    [Required]
    [MaxLength(3)]
    public string Operator { get; set; } = Operator;

    [MaxLength(3)]
    public string? SecondaryOperator { get; set; } = SecondaryOperator;

    [MaxLength(3)]
    public string? TertiaryOperator { get; set; } = TertiaryOperator;

    public int? JBKNumber { get; set; } = JBKNumber;

    [MaxLength(3)]
    public string? LotNumber { get; set; } = LotNumber;

    [MaxLength(3)]
    public string? DieNumber { get; set; } = DieNumber;

    public int? DeburrJBKNumber { get; set; } = DeburrJBKNumber;

    public string? HeatNumber { get; set; } = HeatNumber;

    [Required]
    public string ProductionDate { get; set; } = ProductionDate;
}