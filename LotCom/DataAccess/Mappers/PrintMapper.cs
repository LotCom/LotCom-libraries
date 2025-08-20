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
    /// Maps the values of a Print Dao to a Model object.
    /// </summary>
    /// <param name="Dao"></param>
    /// <returns></returns>
    public static async Task<Print> DaoToModel(PrintDao Dao, UserAgent Agent)
    {
        // retrieve the Process and Part from the Database
        Process? ModelProcess = await ProcessService.Get(Dao.ProcessId, Agent);
        Part? ModelPart = await PartService.Get(Dao.PartId, Agent);
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
        // construct and return new Print Model
        return new Print
        (
            Dao.Id,
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
    /// Maps the values of a Model to a Dao object.
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    public static PrintDao ModelToDao(Print Model)
    {
        // map base data
        PrintDao Dao = new PrintDao
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
            Dao.SecondaryQuantity = Model.SecondaryDataSet.Quantity.Value;
            Dao.SecondaryShift = int.Parse(ShiftExtensions.ToString(Model.SecondaryDataSet.Shift));
            Dao.SecondaryOperator = Model.SecondaryDataSet.Operator.Initials;
        }
        if (Model.TertiaryDataSet is not null)
        {
            Dao.TertiaryQuantity = Model.TertiaryDataSet.Quantity.Value;
            Dao.TertiaryShift = int.Parse(ShiftExtensions.ToString(Model.TertiaryDataSet.Shift));
            Dao.TertiaryOperator = Model.TertiaryDataSet.Operator.Initials;
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