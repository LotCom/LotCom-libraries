namespace LotCom.Database.Transfer;

/// <summary>
/// Defines the publicly-available entity structure of a Process.
/// </summary>
public class ProcessDto(int LineCode, string LineName, string Title, string? Serialization, string Type, bool Origination, string? PassThroughType, bool DoesPrint, bool DoesScan, bool UsesJBKNumber, bool UsesLotNumber, bool UsesDieNumber, bool UsesDeburrJBKNumber, bool UsesHeatNumber, int? Previous1, int? Previous2, int? Previous3, int? Previous4)
{
    public int Id { get; set; } = 0;

    public int LineCode { get; set; } = LineCode;

    public string LineName { get; set; } = LineName;

    public string Title { get; set; } = Title;

    public string? Serialization { get; set; } = Serialization;

    public string Type { get; set; } = Type;

    public bool Origination { get; set; } = Origination;

    public string? PassThroughType { get; set; } = PassThroughType;

    public bool DoesPrint { get; set; } = DoesPrint;

    public bool DoesScan { get; set; } = DoesScan;

    public bool UsesJBKNumber { get; set; } = UsesJBKNumber;

    public bool UsesLotNumber { get; set; } = UsesLotNumber;

    public bool UsesDieNumber { get; set; } = UsesDieNumber;

    public bool UsesDeburrJBKNumber { get; set; } = UsesDeburrJBKNumber;

    public bool UsesHeatNumber { get; set; } = UsesHeatNumber;

    public int? Previous1 { get; set; } = Previous1;

    public int? Previous2 { get; set; } = Previous2;

    public int? Previous3 { get; set; } = Previous3;

    public int? Previous4 { get; set; } = Previous4;
}