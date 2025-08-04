using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// A collection of attributes that define a real-world Part.
/// </summary>
/// <param name="ParentProcess">The Process that the Part is produced by.</param>
/// <param name="PartNumber">The Part Number assigned to the Part.</param>
/// <param name="PartName">The Part Name assigned to the Part.</param>
/// <param name="ModelNumber">The Model Number the Part is associated with.</param>
public partial class Part(int Id, int ParentProcess, string PartNumber, string PartName, ModelNumber ModelNumber) : ObservableObject
{
    /// <summary>
    /// Provides a formatted string to display in a ListView; includes the Part Number and Name.
    /// </summary>
    public string GetInfo => $"{PartNumber}\n  {PartName}";

    /// <summary>
    /// The Id of the Part object in the Database.
    /// </summary>
    public int Id = Id;

    /// <summary>
    /// [Observable] The Process that the Part is assigned to.
    /// </summary>
    [ObservableProperty]
    public partial int ParentProcess { get; set; } = ParentProcess;

    /// <summary>
    /// [Observable] The Part Number assigned to the Part.
    /// </summary>
    [ObservableProperty]
    public partial string PartNumber { get; set; } = PartNumber;

    /// <summary>
    /// [Observable] The Part Name assigned to the Part.
    /// </summary>
    [ObservableProperty]
    public partial string PartName { get; set; } = PartName;

    /// <summary>
    /// [Observable] The Model Number the Part is associated with.
    /// </summary>
    [ObservableProperty]
    public partial ModelNumber ModelNumber { get; set; } = ModelNumber;

    /// <summary>
    /// Converts the object into a string. Uses the Part's PartNumber value as the source for this string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return PartNumber;
    }
}