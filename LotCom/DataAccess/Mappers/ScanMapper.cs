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
    /// Maps the values of a Scan Dto to a Model object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    public static async Task<Scan> DtoToModel(ScanDto Dto, UserAgent Agent)
    {
        Process? ModelScanProcess;
        Process? ModelLabelProcess;
        Part? ModelPart;
        // retrieve the ScanProcess from the Database
        try
        {
            ModelScanProcess = await ProcessService.Get(Dto.ScanProcessId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Process {Dto.ScanProcessId} from the Database.", _ex);
        }
        // some formatting issue
        catch (JsonException _ex)
        {
            throw new DatabaseException("Could not process JSON response.", _ex);
        }
        // retrieve the LabelProcess from the Database
        try
        {
            ModelLabelProcess = await ProcessService.Get(Dto.LabelProcessId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Process {Dto.LabelProcessId} from the Database.", _ex);
        }
        // some formatting issue
        catch (JsonException _ex)
        {
            throw new DatabaseException("Could not process JSON response.", _ex);
        }
        // retrieve the LabelPart from the Database
        try
        {
            ModelPart = await PartService.Get(Dto.PartId, Agent);
        }
        // some database-generated issue
        catch (HttpRequestException _ex)
        {
            throw new DatabaseException($"Could not retreive Part {Dto.PartId} from the Database.", _ex);
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
        if (Dto.JBKNumber is not null)
        {
            ModelVariableFields.JBKNumber = new JBKNumber((int)Dto.JBKNumber);
        }
        if (Dto.LotNumber is not null)
        {
            ModelVariableFields.LotNumber = new LotNumber
            (
                int.Parse(Dto.LotNumber.Replace(" ", ""))
            );
        }
        if (Dto.DieNumber is not null)
        {
            ModelVariableFields.DieNumber = new DieNumber(Dto.DieNumber);
        }
        if (Dto.DeburrJBKNumber is not null)
        {
            ModelVariableFields.DeburrJBKNumber = new JBKNumber((int)Dto.DeburrJBKNumber);
        }
        if (Dto.HeatNumber is not null)
        {
            ModelVariableFields.HeatNumber = new HeatNumber
            (
                int.Parse(Dto.HeatNumber.Replace(" ", ""))
            );
        }
        // map partial data sets
        PartialDataSet PrimaryData = new PartialDataSet
        (
            new Quantity(Dto.Quantity),
            ShiftExtensions.FromString(Dto.Shift.ToString()),
            new Operator(Dto.Operator)
        );
        PartialDataSet? SecondaryData = null;
        PartialDataSet? TertiaryData = null;
        if (Dto.SecondaryQuantity is not null)
        {
            SecondaryData = new PartialDataSet
            (
                new Quantity((int)Dto.SecondaryQuantity),
                ShiftExtensions.FromString(Dto.SecondaryShift.ToString()!),
                new Operator(Dto.SecondaryOperator!)
            );
        }
        if (Dto.TertiaryQuantity is not null)
        {
            TertiaryData = new PartialDataSet
            (
                new Quantity((int)Dto.TertiaryQuantity),
                ShiftExtensions.FromString(Dto.TertiaryShift.ToString()!),
                new Operator(Dto.TertiaryOperator!)
            );
        }
        // convert ProductionDate to DateTime
        string ProductionDate = Dto.ProductionDate
            .Replace("%2F", "/")
            .Replace("%3A", ":");
        DateTime ModelProductionDate = DateTime.ParseExact(ProductionDate, "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        // convert scan date and address to objects
        string ScanDate = Dto.ScanDate
            .Replace("%2F", "/")
            .Replace("%3A", ":");
        DateTime ModelScanDate = DateTime.ParseExact(ScanDate, "MM/dd/yyyy-HH:mm:ss", CultureInfo.InvariantCulture);
        IPAddress ModelScanAddress = IPAddress.Parse(Dto.ScanAddress);
        // construct and return new Scan Model
        return new Scan
        (
            Dto.Id,
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
    /// Maps the values of a Model to a Dto object.
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    public static ScanDto ModelToDto(Scan Model)
    {
        // map base data
        ScanDto Dto = new ScanDto
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
            Dto.SecondaryQuantity = Model.SecondaryData.Quantity.Value;
            Dto.SecondaryShift = int.Parse(ShiftExtensions.ToString(Model.SecondaryData.Shift));
            Dto.SecondaryOperator = Model.SecondaryData.Operator.Initials;
        }
        if (Model.TertiaryData is not null)
        {
            Dto.TertiaryQuantity = Model.TertiaryData.Quantity.Value;
            Dto.TertiaryShift = int.Parse(ShiftExtensions.ToString(Model.TertiaryData.Shift));
            Dto.TertiaryOperator = Model.TertiaryData.Operator.Initials;
        }
        // map variable fields
        if (Model.VariableFields.JBKNumber is not null)
        {
            Dto.JBKNumber = Model.VariableFields.JBKNumber.Literal;
        }
        if (Model.VariableFields.LotNumber is not null)
        {
            Dto.JBKNumber = Model.VariableFields.LotNumber.Literal;
        }
        if (Model.VariableFields.DieNumber is not null)
        {
            Dto.DieNumber = Model.VariableFields.DieNumber.Formatted;
        }
        if (Model.VariableFields.DeburrJBKNumber is not null)
        {
            Dto.DeburrJBKNumber = Model.VariableFields.DeburrJBKNumber.Literal;
        }
        if (Model.VariableFields.HeatNumber is not null)
        {
            Dto.HeatNumber = Model.VariableFields.HeatNumber.ToString();
        }
        // set the ID and return
        Dto.Id = Model.Id;
        return Dto;
    }
}