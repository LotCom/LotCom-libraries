using System.ComponentModel.DataAnnotations;

namespace LotCom.Database.Entities;

/// <summary>
/// Defines the Database entity structure of a Serial Set for a Part.
/// </summary>
public class SerialSetEntity(int PartId, int NextJBK, int NextLot)
{
    [Key]
    public int PartId { get; set; } = PartId;

    [Required]
    public int NextJBK { get; set; } = NextJBK;

    [Required]
    public int NextLot { get; set; } = NextLot;
}