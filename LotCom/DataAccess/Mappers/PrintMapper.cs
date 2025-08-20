using System.Globalization;
using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Types;
using LotCom.Types.Extensions;
using LotCom.Exceptions;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Print Data Transfer objects and Model objects.
/// </summary>
public static class PrintMapper
{
    /// <summary>
    /// Maps the values of a Print Dto to a Model object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    public static async Task<Print> DtoToModel(PrintDto Dto, UserAgent Agent)
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

    /// <summary>
    /// Maps the values of a Model to a Dto object.
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    public static PrintDto ModelToDto(Print Model)
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
}