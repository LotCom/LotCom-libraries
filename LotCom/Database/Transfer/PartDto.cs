namespace LotCom.Database.Transfer;

/// <summary>
/// Defines the publicly-available entity structure of a Part.
/// </summary>
public class PartDto(string Number, int PrintedBy, int ScannedBy, string Name, string ModelCode)
{
    public int Id { get; set; } = 0;

    public string Number { get; set; } = Number;

    public int PrintedBy { get; set; } = PrintedBy;

    public int ScannedBy { get; set; } = ScannedBy;

    public string Name { get; set; } = Name;

    public string ModelCode { get; set; } = ModelCode;
}