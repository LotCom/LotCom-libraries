using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// An objective version of a Part loaded from the Process Masterlist data source.
/// </summary>
/// <param name="ParentProcess">The FullName of the Process that the Part is assigned to.</param>
/// <param name="PartNumber">The Part Number assigned to the Part.</param>
/// <param name="PartName">The Part Name assigned to the Part.</param>
/// <param name="ModelNumber">The Model Number the Part is associated with.</param>
public partial class Part(string ParentProcess, string PartNumber, string PartName, ModelNumber ModelNumber) : ObservableObject
{
    /// <summary>
    /// Provides a formatted string to display in a ListView; includes the Part Number and Name.
    /// </summary>
    public string GetInfo => $"{PartNumber}\n  {PartName}";

    /// <summary>
    /// [Observable] The Process that the Part is assigned to.
    /// </summary>
    [ObservableProperty]
    public partial string ParentProcess { get; set; } = ParentProcess;

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

    /// <summary>
    /// Returns a CSV formatted string representing this Part. Includes the Number and Name of the Part.
    /// </summary>
    /// <returns></returns>
    public string ToCSV()
    {
        return $"{PartNumber},{PartName}";
    }
}