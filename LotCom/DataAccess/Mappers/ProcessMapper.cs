using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Extensions;
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
    public static async Task<Process> DtoToModel(ProcessDto Dto)
    {
        // map native/simple typed properties
        Process Model = new Process
        (
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
        // retrieve printable parts for the Process
        if (Model.Prints)
        {
            Model.PrintParts = await PartService.GetPrintedByProcess(Dto.Id);
        }
        // retrieve scannable parts for the Process
        if (Model.Scans)
        {
            Model.ScanParts = await PartService.GetScannedByProcess(Dto.Id);
        }
        // retrieve any previous Processes for the Process
        if (Dto.Previous1 is not null)
        {
            Process? Previous1 = await ProcessService.Get((int)Dto.Previous1);
            if (Previous1 is not null)
            {
                Model.PreviousProcesses = [Previous1.FullName];
            }
            if (Dto.Previous2 is not null)
            {
                Process? Previous2 = await ProcessService.Get((int)Dto.Previous2);
                if (Previous2 is not null)
                {
                    Model.PreviousProcesses = [Previous2.FullName];
                }
            }
        }
        return Model;
    }

    public static ProcessDto ModelToDto(Process Model)
    {
        throw new NotImplementedException();
    }
}