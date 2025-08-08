using CommunityToolkit.Mvvm.ComponentModel;
using LotCom.Types.Enums;

namespace LotCom.Types;

/// <summary>
/// A collection of attributes that define a real-world Production Process.
/// </summary>
/// <param name="LineCode">The four-digit Process Code assigned to the Process.</param>
/// <param name="Line">The Process' parent Line.</param>
/// <param name="Title">The linguistic title (descriptor) assigned to the Process.</param>
/// <param name="Serialization">The Process' serialization mode.</param>
/// <param name="Type">The Type of Processing that occurs at the Process.</param>
/// <param name="Origination">Whether the Process generates new parts or passes parts from previous Processes.</param>
/// <param name="Prints">Whether this Process Prints Labels or not.</param>
/// <param name="Scans">Whether this Process Scans Labels or not.</param>
/// <param name="PrintParts">The Parts that can be produced and have Labels printed by the Process.</param>
/// <param name="ScanParts">The Parts that can be consumed and Scanned by the Process.</param>
/// <param name="RequiredFields">The data fields required by this Process.</param>
/// <param name="PassThroughType">For Processes with OriginationTypes.PassThrough, the type of Serialization Header to apply to the Label. For Processes with OriginationTypes.Originator, None.</param>
/// <param name="PreviousProcesses">A List of Processes that precede this Process in the Production flow. null for Processes with no previous Processes.</param>
public partial class Process(int Id, int LineCode, string Line, string Title, SerializationMode Serialization, ProcessType Type, OriginationType Origination, bool Prints, bool Scans, List<int>? PrintParts, List<int>? ScanParts, RequiredFields RequiredFields, PassThroughType PassThroughType, List<int>? PreviousProcesses) : ObservableObject()
{
    /// <summary>
    /// The Id of the Process object in the Database.
    /// </summary>
    public int Id = Id;

    /// <summary>
    /// The four-digit Process Code assigned to the Process.
    /// </summary>
    [ObservableProperty]
    public partial int LineCode { get; set; } = LineCode;

    /// <summary>
    /// The Process' parent Line.
    /// </summary>
    [ObservableProperty]
    public partial string Line { get; set; } = Line;

    /// <summary>
    /// The linguistic title (descriptor) assigned to the Process.
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = Title;

    /// <summary>
    /// A reference-friendly Process name in the form of "Code-Title".
    /// </summary>
    [ObservableProperty]
    public partial string FullName { get; set; } = $"{LineCode}-{Line}-{Title}";

    /// <summary>
    /// The Process' serialization mode (JBK || Lot).
    /// </summary>
    [ObservableProperty]
    public partial SerializationMode Serialization { get; set; } = Serialization;

    /// <summary>
    /// The Type of processing that occurs at the Process.
    /// </summary>
    [ObservableProperty]
    public partial ProcessType Type { get; set; } = Type;

    /// <summary>
    /// The Process' serialization type (Originator || Pass-through).
    /// </summary>
    [ObservableProperty]
    public partial OriginationType Origination { get; set; } = Origination;

    /// <summary>
    /// Whether the Process Prints Labels or not.
    /// </summary>
    [ObservableProperty]
    public partial bool Prints { get; set; } = Prints;

    /// <summary>
    /// Whether the Process Scans Labels or not.
    /// </summary>
    [ObservableProperty]
    public partial bool Scans { get; set; } = Scans;

    /// <summary>
    /// The Parts that can be produced and have Labels printed by the Process.
    /// </summary>
    [ObservableProperty]
    public partial IEnumerable<int>? PrintParts { get; set; } = PrintParts;

    /// <summary>
    /// The Parts that can be consumed and Scanned by the Process.
    /// </summary>
    [ObservableProperty]
    public partial IEnumerable<int>? ScanParts { get; set; } = ScanParts;

    /// <summary>
    /// The Production Data fields required at this Process.
    /// </summary>
    [ObservableProperty]
    public partial RequiredFields RequiredFields { get; set; } = RequiredFields;

    /// <summary>
    /// For Processes with Type = "Pass-through", the type of Serialization Header to apply to the Label; "JBK" or "Lot".
    /// For Processes with Type = "Origination", null.
    /// </summary>
    [ObservableProperty]
    public partial PassThroughType PassThroughType { get; set; } = PassThroughType;

    /// <summary>
    /// A List of Processes that precede the Process in the Production Flow.
    /// null for Processes with no previous Process.
    /// </summary>
    [ObservableProperty]
    public partial IEnumerable<int>? PreviousProcesses { get; set; } = PreviousProcesses;

    /// <summary>
    /// Converts the object into a string. Uses the Process' FullName value as the source for this string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return FullName;
    }
}