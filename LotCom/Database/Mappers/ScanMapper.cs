using System.Globalization;
using LotCom.Database.Services;
using System.Net;
using Newtonsoft.Json;
using LotCom.Database.Entities;
using LotCom.Database.Auth;
using LotCom.Core.Models;
using LotCom.Core.Exceptions;
using LotCom.Core.Types;
using LotCom.Core.Extensions;
using LotCom.Database.Transfer;

namespace LotCom.Database.Mappers;

/// <summary>
/// Provides value mapping between Model, Entity, and DTO objects for Scan classes.
/// </summary>
public class ScanMapper : IMapper<Scan, ScanEntity, ScanDto>
{
    public async Task<Scan> DtoToModel(ScanDto Dto, HttpClient Client, UserAgent Agent)
    {
        Process? ModelScanProcess;
        Process? ModelLabelProcess;
        Part? ModelPart;
        // retrieve the ScanProcess from the Database
        try
        {
            ModelScanProcess = await ProcessService.Get(Dto.ScanProcessId, Client, Agent);
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
            ModelLabelProcess = await ProcessService.Get(Dto.LabelProcessId, Client, Agent);
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
            ModelPart = await PartService.Get(Dto.PartId, Client, Agent);
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

    public ScanEntity DtoToEntity(ScanDto Dto)
    {
        ScanEntity Entity = new ScanEntity
        (
            Dto.ScanProcessId,
            Dto.ScanDate
                .Replace("%2F", "/")
                .Replace("%3A", ":"),
            Dto.ScanAddress,
            Dto.LabelProcessId,
            Dto.PartId,
            Dto.Quantity,
            Dto.SecondaryQuantity,
            Dto.TertiaryQuantity,
            Dto.Shift,
            Dto.SecondaryShift,
            Dto.TertiaryShift,
            Dto.Operator,
            Dto.SecondaryOperator,
            Dto.TertiaryOperator,
            Dto.JBKNumber,
            Dto.LotNumber,
            Dto.DieNumber,
            Dto.DeburrJBKNumber,
            Dto.HeatNumber,
            Dto.ProductionDate
                .Replace("%2F", "/")
                .Replace("%3A", ":")
        );
        Entity.Id = Dto.Id;
        return Entity;
    }

    public ScanDto DtoToDto(ScanDto Dto)
    {
        throw new NotImplementedException();
    }

    public ScanDto ModelToDto(Scan Model)
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

    public Scan ModelToModel(Scan Model)
    {
        throw new NotImplementedException();
    }

    public ScanDto EntityToDto(ScanEntity Entity)
    {
        ScanDto Dto = new ScanDto
        (
            Entity.ScanProcessId,
            Entity.ScanDate
                .Replace("/", "%2F")
                .Replace(":", "%3A"),
            Entity.ScanAddress,
            Entity.LabelProcessId,
            Entity.PartId,
            Entity.Quantity,
            Entity.SecondaryQuantity,
            Entity.TertiaryQuantity,
            Entity.Shift,
            Entity.SecondaryShift,
            Entity.TertiaryShift,
            Entity.Operator,
            Entity.SecondaryOperator,
            Entity.TertiaryOperator,
            Entity.JBKNumber,
            Entity.LotNumber,
            Entity.DieNumber,
            Entity.DeburrJBKNumber,
            Entity.HeatNumber,
            Entity.ProductionDate
                .Replace("/", "%2F")
                .Replace(":", "%3A")
        );
        Dto.Id = Entity.Id;
        return Dto;
    }

    public void UpdateEntity(ScanEntity Recipient, ScanEntity Source)
    {
        Recipient.ScanProcessId = Source.ScanProcessId;
        Recipient.ScanDate = Source.ScanDate;
        Recipient.ScanAddress = Source.ScanAddress;
        Recipient.LabelProcessId = Source.LabelProcessId;
        Recipient.PartId = Source.PartId;
        Recipient.Quantity = Source.Quantity;
        Recipient.SecondaryQuantity = Source.SecondaryQuantity;
        Recipient.TertiaryQuantity = Source.TertiaryQuantity;
        Recipient.Shift = Source.Shift;
        Recipient.SecondaryShift = Source.SecondaryShift;
        Recipient.TertiaryShift = Source.TertiaryShift;
        Recipient.Operator = Source.Operator;
        Recipient.SecondaryOperator = Source.SecondaryOperator;
        Recipient.TertiaryOperator = Source.TertiaryOperator;
        Recipient.JBKNumber = Source.JBKNumber;
        Recipient.LotNumber = Source.LotNumber;
        Recipient.DieNumber = Source.DieNumber;
        Recipient.DeburrJBKNumber = Source.DeburrJBKNumber;
        Recipient.HeatNumber = Source.HeatNumber;
        Recipient.ProductionDate = Source.ProductionDate;
    }
}