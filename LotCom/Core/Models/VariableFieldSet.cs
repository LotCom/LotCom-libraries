using CommunityToolkit.Mvvm.ComponentModel;
using LotCom.Core.Types;
using Newtonsoft.Json.Linq;

namespace LotCom.Core.Models;

/// <summary>
/// Provides structured control over the values of each of the data fields in the variable set.
/// </summary>
/// <param name="JBKNumber"></param>
/// <param name="LotNumber"></param>
/// <param name="DeburrJBKNumber"></param>
/// <param name="DieNumber"></param>
/// <param name="HeatNumber"></param>
public partial class VariableFieldSet(JBKNumber? JBKNumber = null, LotNumber? LotNumber = null, JBKNumber? DeburrJBKNumber = null, DieNumber? DieNumber = null, HeatNumber? HeatNumber = null) : ObservableObject()
{
    [ObservableProperty]
    public partial JBKNumber? JBKNumber { get; set; } = JBKNumber;

    [ObservableProperty]
    public partial LotNumber? LotNumber { get; set; } = LotNumber;

    [ObservableProperty]
    public partial JBKNumber? DeburrJBKNumber { get; set; } = DeburrJBKNumber;

    [ObservableProperty]
    public partial DieNumber? DieNumber { get; set; } = DieNumber;

    [ObservableProperty]
    public partial HeatNumber? HeatNumber { get; set; } = HeatNumber;

    /// <summary>
    /// Converts the VariableFieldSet into a JSON stream.
    /// </summary>
    /// <returns></returns>
    public string ToJSON()
    {
        // add each required field to the JSON stream
        string JSON = "{";
        if (JBKNumber is not null)
        {
            JSON += $"\"JBKNumber\":\"{JBKNumber!.Literal}\",";
        }
        if (LotNumber is not null)
        {
            JSON += $"\"LotNumber\":\"{LotNumber!.Literal}\",";
        }
        if (DeburrJBKNumber is not null)
        {
            JSON += $"\"DeburrJBKNumber\":\"{DeburrJBKNumber!.Literal}\",";
        }
        if (DieNumber is not null)
        {
            JSON += $"\"DieNumber\":\"{DieNumber!.Literal}\",";
        }
        if (HeatNumber is not null)
        {
            JSON += $"\"HeatNumber\":\"{HeatNumber!.Literal}\",";
        }
        // remove trailing comma if any field was added
        if (JSON.Length > 1)
        {
            JSON = JSON[..^1];
        }
        JSON += "}";
        return JSON;
    }

    /// <summary>
    /// Attempts to parse a VariableFieldSet from a JSON stream.
    /// </summary>
    /// <param name="Line"></param>
    /// <returns></returns>
    public static VariableFieldSet ParseJSON(string Line)
    {
        // parse Line into JTokens
        JObject JSON = JObject.Parse(Line);
        // attempt to parse each of the field types
        VariableFieldSet VariableFields = new VariableFieldSet();
        try
        {
            VariableFields.JBKNumber = new JBKNumber(int.Parse(JSON["JBKNumber"]!.ToString()));
        }
        catch
        {
            VariableFields.JBKNumber = null;
        }
        try
        {
            VariableFields.LotNumber = new LotNumber(int.Parse(JSON["LotNumber"]!.ToString()));
        }
        catch
        {
            VariableFields.LotNumber = null;
        }
        try
        {
            VariableFields.DeburrJBKNumber = new JBKNumber(int.Parse(JSON["DeburrJBKNumber"]!.ToString()));
        }
        catch
        {
            VariableFields.DeburrJBKNumber = null;
        }
        try
        {
            VariableFields.DieNumber = new DieNumber(JSON["DieNumber"]!.ToString());
        }
        catch
        {
            VariableFields.DieNumber = null;
        }
        try
        {
            VariableFields.HeatNumber = new HeatNumber(int.Parse(JSON["HeatNumber"]!.ToString()));
        }
        catch
        {
            VariableFields.HeatNumber = null;
        }
        return VariableFields;
    }

    /// <summary>
    /// Converts the object to a CSV formatted string. Lists each of the non-null properties in the standard order of fields. 
    /// Can return an empty string if none of the Variable Fields contain non-null values.
    /// </summary>
    /// <returns></returns>
    public string ToCSV()
    {
        string CSVLine = "";
        // only add values if they are non-null
        if (JBKNumber is not null && !JBKNumber.ToString().Equals(""))
        {
            CSVLine += JBKNumber.ToString() + ",";
        }
        if (LotNumber is not null && !LotNumber.ToString().Equals(""))
        {
            CSVLine += LotNumber.ToString() + ",";
        }
        if (DeburrJBKNumber is not null && !DeburrJBKNumber.ToString().Equals(""))
        {
            CSVLine += DeburrJBKNumber.ToString() + ",";
        }
        if (DieNumber is not null && !DieNumber.ToString().Equals(""))
        {
            CSVLine += DieNumber.ToString() + ",";
        }
        if (HeatNumber is not null && !HeatNumber.ToString().Equals(""))
        {
            CSVLine += HeatNumber.ToString() + ",";
        }
        // return the final product without a trailing comma 
        if (CSVLine.Length > 0)
        {
            CSVLine = CSVLine[..^1];
        }
        return CSVLine;
    }

    /// <summary>
    /// Attempts to convert a
    /// </summary>
    /// <param name="StringFields"></param>
    /// <param name="Required"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static VariableFieldSet ParseCSV(string[] StringFields, RequiredFields Required)
    {
        // confirm that the passed field set is not too long
        if (StringFields.Length > 6)
        {
            throw new ArgumentException($"VariableFieldSets cannot exceed 6 fields (passed {StringFields.Length}).");
        }
        int Offset = 0;
        VariableFieldSet ParsedSet = new VariableFieldSet();
        // parse a JBK # if required
        if (Required.JBKNumber)
        {
            try
            {
                ParsedSet.JBKNumber = new JBKNumber(int.Parse(StringFields[Offset]));
                Offset += 1;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Failed to create a JBK Number from the required value {StringFields[Offset]}.");
            }
        }
        // parse a Lot # if required
        if (Required.LotNumber)
        {
            try
            {
                ParsedSet.LotNumber = new LotNumber(int.Parse(StringFields[Offset]));
                Offset += 1;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Failed to create a Lot Number from the required value {StringFields[Offset]}.");
            }
        }
        // parse a Deburr JBK # if required
        if (Required.DeburrJBKNumber)
        {
            try
            {
                ParsedSet.DeburrJBKNumber = new JBKNumber(int.Parse(StringFields[Offset]));
                Offset += 1;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Failed to create a JBK Number from the required value {StringFields[Offset]}.");
            }
        }
        // parse a Die # if required
        if (Required.DieNumber)
        {
            try
            {
                ParsedSet.DieNumber = new DieNumber(StringFields[Offset]);
                Offset += 1;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Failed to create a Die Number from the required value {StringFields[Offset]}.");
            }
        }
        // parse a Heat # if required
        if (Required.HeatNumber)
        {
            try
            {
                ParsedSet.HeatNumber = new HeatNumber(int.Parse(StringFields[Offset]));
                Offset += 1;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Failed to create a Heat Number from the required value {StringFields[Offset]}.");
            }
        }
        return ParsedSet;
    }
}