using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Types.Extensions;
using LotCom.Types;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Process Data Transfer objects and Model objects.
/// </summary>
public static class ProcessMapper
{
    /// <summary>
    /// Maps the values of a Process Dao to a Model object.
    /// </summary>
    /// <param name="Dao"></param>
    /// <returns></returns>
    public static async Task<Process> DaoToModel(ProcessDao Dao, UserAgent Agent)
    {
        // map native/simple typed properties
        Process Model = new Process
        (
            Dao.Id,
            Dao.LineCode,
            Dao.LineName,
            Dao.Title,
            SerializationModeExtensions.FromString(Dao.Serialization!),
            ProcessTypeExtensions.FromString(Dao.Type),
            OriginationTypeExtensions.FromBoolean(Dao.Origination),
            Dao.DoesPrint,
            Dao.DoesScan,
            null,
            null,
            new RequiredFields
            (
                Dao.UsesJBKNumber,
                Dao.UsesLotNumber,
                Dao.UsesDieNumber,
                Dao.UsesDeburrJBKNumber,
                Dao.UsesHeatNumber
            ),
            PassThroughTypeExtensions.FromString(Dao.PassThroughType!),
            null
        );
        // retrieve printable part ids for the Process
        if (Model.Prints)
        {
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetPrintedByProcess(Dao.Id, Agent);
            if (PartsFromDatabase is null)
            {
                Model.PrintParts = [];
            }
            else
            {
                Model.PrintParts = PartsFromDatabase.Select(x => x.Id);
            }
        }
        // retrieve scannable part ids for the Process
        if (Model.Scans)
        {
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetScannedByProcess(Dao.Id, Agent);
            if (PartsFromDatabase is null)
            {
                Model.ScanParts = [];
            }
            else
            {
                Model.ScanParts = PartsFromDatabase.Select(x => x.Id);
            }
        }
        // retrieve any previous Processes for the Process
        if (Dao.Previous1 is not null)
        {
            Model.PreviousProcesses = [(int)Dao.Previous1];
            if (Dao.Previous2 is not null)
            {
                Model.PreviousProcesses = Model.PreviousProcesses.Append((int)Dao.Previous2);
            }
        }
        return Model;
    }

    public static ProcessDao ModelToDao(Process Model)
    {
        throw new NotImplementedException();
    }
}