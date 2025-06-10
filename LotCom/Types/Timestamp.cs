namespace LotCom.Types;

/// <summary>
/// Formats a DateTime object as a string like MM/DD/YYYY-HH:MM:SS.
/// </summary>
public class Timestamp
{
    /// <summary>
    /// The Timestamp created by this class.
    /// </summary>
    public string Stamp = "";

    /// <summary>
    /// Create a Timestamp object from Date.
    /// </summary>
    /// <param name="Date"></param>
    public Timestamp(DateTime Date)
    {
        // retrieve and format each timestamp field
        string Month = $"{Date.Month}";
        if (Month.Length < 2)
        {
            Month = "0" + Month;
        }
        string Day = $"{Date.Day}";
        if (Day.Length < 2)
        {
            Day = "0" + Day;
        }
        string Year = $"{Date.Year}";
        string Hour = $"{Date.Hour}";
        if (Hour.Length < 2)
        {
            Hour = "0" + Hour;
        }
        string Minute = $"{Date.Minute}";
        if (Minute.Length < 2)
        {
            Minute = "0" + Minute;
        }
        string Second = $"{Date.Second}";
        if (Second.Length < 2)
        {
            Second = "0" + Second;
        }
        // set the Stamp property to the timestamp as a string
        Stamp = $"{Month}/{Day}/{Year}-{Hour}:{Minute}:{Second}";
    }
}