using System.Globalization;
using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Types;
using LotCom.Types.Extensions;
using LotCom.Exceptions;
using LotCom.DataAccess.Entities;
using LotCom.DataAccess.Auth;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Model, Entity, and DTO objects for Print classes.
/// </summary>
public class PrintMapper : IMapper<Print, PrintEntity, PrintDto>
{
    public async Task<Print> DtoToModel(PrintDto Dto, UserAgent Agent)
    {
        // retrieve the Process and Part from the Database
        Process? ModelProcess = await ProcessService.Get(Dto.ProcessId, Agent);
        Part? ModelPart = await PartService.Get(Dto.PartId, Agent);
        if (ModelProcess is null)
        {
            throw new DatabaseException("Could not retrieve the Process referenced by the Print.");
        }
        if (ModelPart is null)
        {
            throw new DatabaseException("Could not retrieve the Part referenced by the Print.");
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
        // construct and return new Print Model
        return new Print
        (
            Dto.Id,
            ModelProcess,
            ModelPart,
            ModelVariableFields,
            ModelProductionDate,
            PrimaryData,
            SecondaryData,
            TertiaryData
        );
    }

    public PrintEntity DtoToEntity(PrintDto Dto)
    {
        PrintEntity Mapped = new PrintEntity
        (
            ProcessId: Dto.ProcessId,
            PartId: Dto.PartId,
            Quantity: Dto.Quantity,
            SecondaryQuantity: Dto.SecondaryQuantity,
            TertiaryQuantity: Dto.TertiaryQuantity,
            Shift: Dto.Shift,
            SecondaryShift: Dto.SecondaryShift,
            TertiaryShift: Dto.TertiaryShift,
            Operator: Dto.Operator,
            SecondaryOperator: Dto.SecondaryOperator,
            TertiaryOperator: Dto.TertiaryOperator,
            JBKNumber: Dto.JBKNumber,
            LotNumber: Dto.LotNumber,
            DieNumber: Dto.DieNumber,
            DeburrJBKNumber: Dto.DeburrJBKNumber,
            HeatNumber: Dto.HeatNumber,
            ProductionDate: Dto.ProductionDate
        );
        Mapped.Id = Dto.Id;
        return Mapped;
    }

    public PrintDto DtoToDto(PrintDto Dto)
    {
        throw new NotImplementedException();
    }

    public PrintDto ModelToDto(Print Model)
    {
        // map base data
        PrintDto Dto = new PrintDto
        (
            Model.Process.Id,
            Model.Part.Id,
            Model.PrimaryDataSet.Quantity.Value,
            null,
            null,
            int.Parse(ShiftExtensions.ToString(Model.PrimaryDataSet.Shift)),
            null,
            null,
            Model.PrimaryDataSet.Operator.Initials,
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
        if (Model.SecondaryDataSet is not null)
        {
            Dto.SecondaryQuantity = Model.SecondaryDataSet.Quantity.Value;
            Dto.SecondaryShift = int.Parse(ShiftExtensions.ToString(Model.SecondaryDataSet.Shift));
            Dto.SecondaryOperator = Model.SecondaryDataSet.Operator.Initials;
        }
        if (Model.TertiaryDataSet is not null)
        {
            Dto.TertiaryQuantity = Model.TertiaryDataSet.Quantity.Value;
            Dto.TertiaryShift = int.Parse(ShiftExtensions.ToString(Model.TertiaryDataSet.Shift));
            Dto.TertiaryOperator = Model.TertiaryDataSet.Operator.Initials;
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

    public Print ModelToModel(Print Model)
    {
        throw new NotImplementedException();
    }

    public PrintDto EntityToDto(PrintEntity Entity)
    {
        PrintDto Mapped = new PrintDto
        (
            ProcessId: Entity.ProcessId,
            PartId: Entity.PartId,
            Quantity: Entity.Quantity,
            SecondaryQuantity: Entity.SecondaryQuantity,
            TertiaryQuantity: Entity.TertiaryQuantity,
            Shift: Entity.Shift,
            SecondaryShift: Entity.SecondaryShift,
            TertiaryShift: Entity.TertiaryShift,
            Operator: Entity.Operator,
            SecondaryOperator: Entity.SecondaryOperator,
            TertiaryOperator: Entity.TertiaryOperator,
            JBKNumber: Entity.JBKNumber,
            LotNumber: Entity.LotNumber,
            DieNumber: Entity.DieNumber,
            DeburrJBKNumber: Entity.DeburrJBKNumber,
            HeatNumber: Entity.HeatNumber,
            ProductionDate: Entity.ProductionDate
        );
        Mapped.Id = Entity.Id;
        return Mapped;
    }

    public PrintEntity EntityToEntity(PrintEntity Entity)
    {
        PrintEntity New = new PrintEntity
        (
            Entity.ProcessId,
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
        );
        return New;
    }
}