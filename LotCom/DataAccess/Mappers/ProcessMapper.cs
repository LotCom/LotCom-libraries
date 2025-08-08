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
    /// Maps the values of a Process DTO to a Model object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    public static async Task<Process> DtoToModel(ProcessDto Dto, UserAgent Agent)
    {
        // map native/simple typed properties
        Process Model = new Process
        (
            Dto.Id,
            Dto.LineCode,
            Dto.LineName,
            Dto.Title,
            SerializationModeExtensions.FromString(Dto.Serialization),
            ProcessTypeExtensions.FromString(Dto.Type),
            OriginationTypeExtensions.FromBoolean(Dto.Origination),
            Dto.DoesPrint,
            Dto.DoesScan,
            null,
            null,
            new RequiredFields
            (
                Dto.UsesJBKNumber,
                Dto.UsesLotNumber,
                Dto.UsesDieNumber,
                Dto.UsesDeburrJBKNumber,
                Dto.UsesHeatNumber
            ),
            PassThroughTypeExtensions.FromString(Dto.PassThroughType!),
            null
        );
        // retrieve printable part ids for the Process
        if (Model.Prints)
        {
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetPrintedByProcess(Dto.Id, Agent);
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
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetScannedByProcess(Dto.Id, Agent);
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
        if (Dto.Previous1 is not null)
        {
            Model.PreviousProcesses = [(int)Dto.Previous1];
            if (Dto.Previous2 is not null)
            {
                Model.PreviousProcesses = Model.PreviousProcesses.Append((int)Dto.Previous2);
            }
        }
        return Model;
    }

    public static ProcessDto ModelToDto(Process Model)
    {
        throw new NotImplementedException();
    }
}