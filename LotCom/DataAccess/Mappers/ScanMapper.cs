using System.Globalization;
using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Types;
using LotCom.Types.Extensions;
using LotCom.Exceptions;
using System.Net;
using Newtonsoft.Json;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Scan Data Transfer objects and Model objects.
/// </summary>
public static class ScanMapper
{
    /// <summary>
    /// Maps the values of a Scan Dao to a Model object.
    /// </summary>
    /// <param name="Dao"></param>
    /// <returns></returns>
    public static async Task<Scan> DaoToModel(ScanDao Dao, UserAgent Agent)
    {
        Process? ModelScanProcess;
        Process? ModelLabelProcess;
        Part? ModelPart;
        // retrieve the ScanProcess from the Database
        try
        {
            ModelScanProcess = await ProcessService.Get(Dao.ScanProcessId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Process {Dao.ScanProcessId} from the Database.", _ex);
        }
        // some formatting issue
        catch (JsonException _ex)
        {
            throw new DatabaseException("Could not process JSON response.", _ex);
        }
        // retrieve the LabelProcess from the Database
        try
        {
            ModelLabelProcess = await ProcessService.Get(Dao.LabelProcessId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Process {Dao.LabelProcessId} from the Database.", _ex);
        }
        // some formatting issue
        catch (JsonException _ex)
        {
            throw new DatabaseException("Could not process JSON response.", _ex);
        }
        // retrieve the LabelPart from the Database
        try
        {
            ModelPart = await PartService.Get(Dao.PartId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Part {Dao.PartId} from the Database.", _ex);
        }
        // some formatting issue
        catch (JsonException _ex)
        {
            throw new DatabaseException("Could not process JSON response.", _ex);
        }
        if (ModelScanProcess is null)
        {
            throw new DatabaseException("Could not retrieve the Process that created the Scan.");
        }
        if (ModelLabelProcess is null)
        {
            throw new DatabaseException("Could not retrieve the Process referenced by the Scan.");
        }
        if (ModelPart is null)
        {
            throw new DatabaseException("Could not retrieve the Part referenced by the Scan.");
        }
        // map any non-null variable fields
        VariableFieldSet ModelVariableFields = new VariableFieldSet();
        if (Dao.JBKNumber is not null)
        {
            ModelVariableFields.JBKNumber = new JBKNumber((int)Dao.JBKNumber);
        }
        if (Dao.LotNumber is not null)
        {
            ModelVariableFields.LotNumber = new LotNumber
            (
                int.Parse(Dao.LotNumber.Replace(" ", ""))
            );
        }
        if (Dao.DieNumber is not null)
        {
            ModelVariableFields.DieNumber = new DieNumber(Dao.DieNumber);
        }
        if (Dao.DeburrJBKNumber is not null)
        {
            ModelVariableFields.DeburrJBKNumber = new JBKNumber((int)Dao.DeburrJBKNumber);
        }
        if (Dao.HeatNumber is not null)
        {
            ModelVariableFields.HeatNumber = new HeatNumber
            (
                int.Parse(Dao.HeatNumber.Replace(" ", ""))
            );
        }
        // map partial data sets
        PartialDataSet PrimaryData = new PartialDataSet
        (
            new Quantity(Dao.Quantity),
            ShiftExtensions.FromString(Dao.Shift.ToString()),
            new Operator(Dao.Operator)
        );
        PartialDataSet? SecondaryData = null;
        PartialDataSet? TertiaryData = null;
        if (Dao.SecondaryQuantity is not null)
        {
            SecondaryData = new PartialDataSet
            (
                new Quantity((int)Dao.SecondaryQuantity),
                ShiftExtensions.FromString(Dao.SecondaryShift.ToString()!),
                new Operator(Dao.SecondaryOperator!)
            );
        }
        if (Dao.TertiaryQuantity is not null)
        {
            TertiaryData = new PartialDataSet
            (
                new Quantity((int)Dao.TertiaryQuantity),
                ShiftExtensions.FromString(Dao.TertiaryShift.ToString()!),
                new Operator(Dao.TertiaryOperator!)
            );
        }
        // convert ProductionDate to DateTime
        string ProductionDate = Dao.ProductionDate
            .Replace("%2F", "/")
            .Replace("%3A", ":");
        DateTime ModelProductionDate = DateTime.ParseExact(ProductionDate, "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        // convert scan date and address to objects
        string ScanDate = Dao.ScanDate
            .Replace("%2F", "/")
            .Replace("%3A", ":");
        DateTime ModelScanDate = DateTime.ParseExact(ScanDate, "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        IPAddress ModelScanAddress = IPAddress.Parse(Dao.ScanAddress);
        // construct and return new Scan Model
        return new Scan
        (
            Dao.Id,
            ModelScanProcess,
            ModelScanDate,
            ModelScanAddress,
            ModelLabelProcess,
            ModelPart,
            ModelVariableFields,
            ModelProductionDate,
            PrimaryData,
            SecondaryData,
            TertiaryData
        );
    }

    /// <summary>
    /// Maps the values of a Model to a Dao object.
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    public static ScanDao ModelToDao(Scan Model)
    {
        // map base data
        ScanDao Dao = new ScanDao
        (
            Model.ScanProcess.Id,
            new Timestamp(Model.ScanDate).Stamp
                .Replace("/", "%2F")
                .Replace(":", "%3A"),
            Model.ScanAddress.ToString(),
            Model.LabelProcess.Id,
            Model.Part.Id,
            Model.PrimaryData.Quantity.Value,
            null,
            null,
            int.Parse(ShiftExtensions.ToString(Model.PrimaryData.Shift)),
            null,
            null,
            Model.PrimaryData.Operator.Initials,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new Timestamp(Model.ProductionDate).Stamp
                .Replace("/", "%2F")
                .Replace(":", "%3A")
        );
        // map secondary and tertiary data sets
        if (Model.SecondaryData is not null)
        {
            Dao.SecondaryQuantity = Model.SecondaryData.Quantity.Value;
            Dao.SecondaryShift = int.Parse(ShiftExtensions.ToString(Model.SecondaryData.Shift));
            Dao.SecondaryOperator = Model.SecondaryData.Operator.Initials;
        }
        if (Model.TertiaryData is not null)
        {
            Dao.TertiaryQuantity = Model.TertiaryData.Quantity.Value;
            Dao.TertiaryShift = int.Parse(ShiftExtensions.ToString(Model.TertiaryData.Shift));
            Dao.TertiaryOperator = Model.TertiaryData.Operator.Initials;
        }
        // map variable fields
        if (Model.VariableFields.JBKNumber is not null)
        {
            Dao.JBKNumber = Model.VariableFields.JBKNumber.Literal;
        }
        if (Model.VariableFields.LotNumber is not null)
        {
            Dao.JBKNumber = Model.VariableFields.LotNumber.Literal;
        }
        if (Model.VariableFields.DieNumber is not null)
        {
            Dao.DieNumber = Model.VariableFields.DieNumber.Formatted;
        }
        if (Model.VariableFields.DeburrJBKNumber is not null)
        {
            Dao.DeburrJBKNumber = Model.VariableFields.DeburrJBKNumber.Literal;
        }
        if (Model.VariableFields.HeatNumber is not null)
        {
            Dao.HeatNumber = Model.VariableFields.HeatNumber.ToString();
        }
        // set the ID and return
        Dao.Id = Model.Id;
        return Dao;
    }
}