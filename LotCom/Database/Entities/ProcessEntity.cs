using System.ComponentModel.DataAnnotations;

namespace LotCom.Database.Entities;

/// <summary>
/// Defines the Database entity structure of a Process.
/// </summary>
public class ProcessEntity(int LineCode, string LineName, string Title, string? Serialization, string Type, bool Origination, string? PassThroughType, bool DoesPrint, bool DoesScan, bool UsesJBKNumber, bool UsesLotNumber, bool UsesDieNumber, bool UsesDeburrJBKNumber, bool UsesHeatNumber, int? Previous1, int? Previous2, int? Previous3, int? Previous4)
{
    // sensitive properties (not transfered by DTO Layer)
    [MaxLength(20)]
    public string Created { get; set; } = "";

    [MaxLength(20)]
    public string Updated { get; set; } = "";

    // open properties (transfered by DTO Layer)
    [Key]
    public int Id { get; set; } = 0;
    
    [Required]
    public int LineCode { get; set; } = LineCode;

    [Required]
    [MaxLength(10)]
    public string LineName { get; set; } = LineName;

    [Required]
    [MaxLength(20)]
    public string Title { get; set; } = Title;

    [MaxLength(10)]
    public string? Serialization { get; set; } = Serialization;

    [Required]
    [MaxLength(15)]
    public string Type { get; set; } = Type;

    [Required]
    public bool Origination { get; set; } = Origination;

    [MaxLength(3)]
    public string? PassThroughType { get; set; } = PassThroughType;

    [Required]
    public bool DoesPrint { get; set; } = DoesPrint;

    [Required]
    public bool DoesScan { get; set; } = DoesScan;

    [Required]
    public bool UsesJBKNumber { get; set; } = UsesJBKNumber;

    [Required]
    public bool UsesLotNumber { get; set; } = UsesLotNumber;

    [Required]
    public bool UsesDieNumber { get; set; } = UsesDieNumber;

    [Required]
    public bool UsesDeburrJBKNumber { get; set; } = UsesDeburrJBKNumber;

    [Required]
    public bool UsesHeatNumber { get; set; } = UsesHeatNumber;

    public int? Previous1 { get; set; } = Previous1;

    public int? Previous2 { get; set; } = Previous2;

    public int? Previous3 { get; set; } = Previous3;

    public int? Previous4 { get; set; } = Previous4;
}