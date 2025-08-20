using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.Types;

/// <summary>
/// Provides structured control over the requirement of data fields in the variable set.
/// </summary>
/// <param name="JBKNumber"></param>
/// <param name="LotNumber"></param>
/// <param name="DieNumber"></param>
/// <param name="DeburrJBKNumber"></param>
/// <param name="HeatNumber"></param>
public partial class RequiredFields(bool JBKNumber = false, bool LotNumber = false, bool DieNumber = false, bool DeburrJBKNumber = false, bool HeatNumber = false) : ObservableObject()
{
    [ObservableProperty]
    public partial bool JBKNumber { get; set; } = JBKNumber;

    [ObservableProperty]
    public partial bool LotNumber { get; set; } = LotNumber;

    [ObservableProperty]
    public partial bool DieNumber { get; set; } = DieNumber;

    [ObservableProperty]
    public partial bool DeburrJBKNumber { get; set; } = DeburrJBKNumber;

    [ObservableProperty]
    public partial bool HeatNumber { get; set; } = HeatNumber;

    /// <summary>
    /// Attempts to parse a RequiredFields object from a JSON stream.
    /// </summary>
    /// <param name="Line"></param>
    /// <returns></returns>
    public static RequiredFields ParseJSON(string Line)
    {
        RequiredFields Requirements = new RequiredFields();
        // attempt to parse each of the field requirements
        if (Line.Contains("JBKNumber"))
        {
            Requirements.JBKNumber = true;
        }
        if (Line.Contains("LotNumber"))
        {
            Requirements.LotNumber = true;
        }
        if (Line.Contains("DieNumber"))
        {
            Requirements.DieNumber = true;
        }
        if (Line.Contains("DeburrJBKNumber"))
        {
            Requirements.DeburrJBKNumber = true;
        }
        if (Line.Contains("HeatNumber"))
        {
            Requirements.HeatNumber = true;
        }
        return Requirements;
    }
}