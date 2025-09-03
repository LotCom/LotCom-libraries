using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LotCom.Database.Entities;

/// <summary>
/// Defines the Database entity structure of a Scan.
/// </summary>
public class ScanEntity(int ScanProcessId, string ScanDate, string ScanAddress, int LabelProcessId, int PartId, int Quantity, int? SecondaryQuantity, int? TertiaryQuantity, int Shift, int? SecondaryShift, int? TertiaryShift, string Operator, string? SecondaryOperator, string? TertiaryOperator, int? JBKNumber, string? LotNumber, string? DieNumber, int? DeburrJBKNumber, string? HeatNumber, string ProductionDate)
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
    public int ScanProcessId { get; set; } = ScanProcessId;

    [Required]
    [MaxLength(20)]
    public string ScanDate { get; set; } = ScanDate;

    [Required]
    [MaxLength(15)]
    public string ScanAddress { get; set; } = ScanAddress;

    [Required]
    public int LabelProcessId { get; set; } = LabelProcessId;

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
    
    /// <summary>
    /// Returns the ScanEntity's serial number, as an unformatted literal.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public int? GetSerialNumber()
    {
        // use the JBK number
        if (JBKNumber is not null)
        {
            return (int)JBKNumber;
        }
        // use the Lot number
        else if (LotNumber is not null)
        {
            return int.Parse(LotNumber);
        }
        // use the DeburrJBKNumber
        else if (DeburrJBKNumber is not null)
        {
            return (int)DeburrJBKNumber;
        }
        // no number available
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if DateToCompare is within 0 and RangeInDays days before the Created Date of this Scan.
    /// </summary>
    /// <param name="RangeInDays"></param>
    /// <param name="DateToCompare"></param>
    /// <returns></returns>
    public bool CompareDateWithinRange(int RangeInDays, DateTime DateToCompare)
    {
        // convert this entity Created Date to a Datetime and subtract the comparison date
        DateTime ThisDate = DateTime.ParseExact(Created, "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        TimeSpan Difference = DateToCompare.Subtract(ThisDate);
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