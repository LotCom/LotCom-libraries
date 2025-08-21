using System.ComponentModel.DataAnnotations;

namespace LotCom.Database.Entities;

/// <summary>
/// Defines the Database entity structure of a Part.
/// </summary>
public class PartEntity(string Number, int PrintedBy, int ScannedBy, string Name, string ModelCode)
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
    [MaxLength(25)]
    public string Number { get; set; } = Number;

    [Required]
    public int PrintedBy { get; set; } = PrintedBy;

    [Required]
    public int ScannedBy { get; set; } = ScannedBy;

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = Name;

    [Required]
    [MaxLength(4)]
    public string ModelCode { get; set; } = ModelCode;
}