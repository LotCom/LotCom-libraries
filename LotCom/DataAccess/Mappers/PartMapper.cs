using LotCom.Core.Models;
using LotCom.Core.Types;
using LotCom.DataAccess.Auth;
using LotCom.DataAccess.Entities;
using LotCom.DataAccess.Models;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Model, Entity, and DTO objects for Part classes.
/// </summary>
public class PartMapper : IMapper<Part, PartEntity, PartDto>
{
    public async Task<Part> DtoToModel(PartDto Dto, UserAgent Agent)
    {
        return await Task.Run(() =>
        {
            return new Part
            (
                Dto.Id,
                Dto.PrintedBy,
                Dto.Number,
                Dto.Name,
                new ModelNumber(Dto.ModelCode)
            );
        });
    }

    public PartEntity DtoToEntity(PartDto Dto)
    {
        PartEntity Entity = new PartEntity
        (
            Dto.Number,
            Dto.PrintedBy,
            Dto.ScannedBy,
            Dto.Name,
            Dto.ModelCode
        );
        Entity.Id = Dto.Id;
        return Entity;
    }

    public PartDto DtoToDto(PartDto Dto)
    {
        throw new NotImplementedException();
    }

    public PartDto ModelToDto(Part Model)
    {
        throw new NotImplementedException();
    }

    public Part ModelToModel(Part Model)
    {
        throw new NotImplementedException();
    }

    public PartDto EntityToDto(PartEntity Entity)
    {
        PartDto Dto = new PartDto
        (
            Entity.Number,
            Entity.PrintedBy,
            Entity.ScannedBy,
            Entity.Name,
            Entity.ModelCode
        );
        Dto.Id = Entity.Id;
        return Dto;
    }

    public void UpdateEntity(PartEntity Recipient, PartEntity Source)
    {
        Recipient.Number = Source.Number;
        Recipient.PrintedBy = Source.PrintedBy;
        Recipient.ScannedBy = Source.ScannedBy;
        Recipient.Name = Source.Name;
        Recipient.ModelCode = Source.ModelCode;
    }
}