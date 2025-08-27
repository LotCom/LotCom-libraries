using LotCom.Database.Services;
using LotCom.Database.Auth;
using LotCom.Database.Entities;
using LotCom.Core.Models;
using LotCom.Core.Extensions;
using LotCom.Database.Transfer;

namespace LotCom.Database.Mappers;

/// <summary>
/// Provides value mapping between Model, Entity, and DTO objects for Process classes.
/// </summary>
public class ProcessMapper : IMapper<Process, ProcessEntity, ProcessDto>
{
    public async Task<Process> DtoToModel(ProcessDto Dto, HttpClient Client, UserAgent Agent)
    {
        // map native/simple typed properties
        Process Model = new Process
        (
            Dto.Id,
            Dto.LineCode,
            Dto.LineName,
            Dto.Title,
            SerializationModeExtensions.FromString(Dto.Serialization!),
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
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetPrintedByProcess(Dto.Id, Client, Agent);
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
            IEnumerable<Part>? PartsFromDatabase = await PartService.GetScannedByProcess(Dto.Id, Client, Agent);
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
                if (Dto.Previous3 is not null)
                {
                    Model.PreviousProcesses = Model.PreviousProcesses.Append((int)Dto.Previous3);
                    if (Dto.Previous4 is not null)
                    {
                        Model.PreviousProcesses = Model.PreviousProcesses.Append((int)Dto.Previous4);
                    }
                }
            }
        }
        return Model;
    }

    public ProcessEntity DtoToEntity(ProcessDto Dto)
    {
        ProcessEntity Entity = new ProcessEntity
        (
            Dto.LineCode,
            Dto.LineName,
            Dto.Title,
            Dto.Serialization,
            Dto.Type,
            Dto.Origination,
            Dto.PassThroughType,
            Dto.DoesPrint,
            Dto.DoesScan,
            Dto.UsesJBKNumber,
            Dto.UsesLotNumber,
            Dto.UsesDieNumber,
            Dto.UsesDeburrJBKNumber,
            Dto.UsesHeatNumber,
            Dto.Previous1,
            Dto.Previous2,
            Dto.Previous3,
            Dto.Previous4
        );
        Entity.Id = Dto.Id;
        return Entity;
    }

    public ProcessDto DtoToDto(ProcessDto Dto)
    {
        throw new NotImplementedException();
    }

    public ProcessDto ModelToDto(Process Model)
    {
        throw new NotImplementedException();
    }

    public Process ModelToModel(Process Model)
    {
        throw new NotImplementedException();
    }

    public ProcessDto EntityToDto(ProcessEntity Entity)
    {
        ProcessDto Dto = new ProcessDto
        (
            Entity.LineCode,
            Entity.LineName,
            Entity.Title,
            Entity.Serialization,
            Entity.Type,
            Entity.Origination,
            Entity.PassThroughType,
            Entity.DoesPrint,
            Entity.DoesScan,
            Entity.UsesJBKNumber,
            Entity.UsesLotNumber,
            Entity.UsesDieNumber,
            Entity.UsesDeburrJBKNumber,
            Entity.UsesHeatNumber,
            Entity.Previous1,
            Entity.Previous2,
            Entity.Previous3,
            Entity.Previous4
        );
        Dto.Id = Entity.Id;
        return Dto;
    }

    public void UpdateEntity(ProcessEntity Recipient, ProcessEntity Source)
    {
        Recipient.LineCode = Source.LineCode;
        Recipient.LineName = Source.LineName;
        Recipient.Title = Source.Title;
        Recipient.Serialization = Source.Serialization;
        Recipient.Type = Source.Type;
        Recipient.Origination = Source.Origination;
        Recipient.PassThroughType = Source.PassThroughType;
        Recipient.DoesPrint = Source.DoesPrint;
        Recipient.DoesScan = Source.DoesScan;
        Recipient.UsesJBKNumber = Source.UsesJBKNumber;
        Recipient.UsesLotNumber = Source.UsesLotNumber;
        Recipient.UsesDieNumber = Source.UsesDieNumber;
        Recipient.UsesDeburrJBKNumber = Source.UsesDeburrJBKNumber;
        Recipient.UsesHeatNumber = Source.UsesHeatNumber;
        Recipient.Previous1 = Source.Previous1;
        Recipient.Previous2 = Source.Previous2;
        Recipient.Previous3 = Source.Previous3;
        Recipient.Previous4 = Source.Previous4;
    }
}