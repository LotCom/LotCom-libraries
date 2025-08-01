namespace LotCom.DataAccess.Models;

public class PartDto(int Id, string Number, int PrintedBy, int ScannedBy, string Name, string ModelCode)
{
    public int Id { get; set; } = Id;
    public string Number { get; set; } = Number;
    public int PrintedBy { get; set; } = PrintedBy;
    public int ScannedBy { get; set; } = ScannedBy;
    public string Name { get; set; } = Name;
    public string ModelCode { get; set; } = ModelCode;
}