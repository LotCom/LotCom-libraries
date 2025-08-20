using LotCom.DataAccess.Models;
using LotCom.Types;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Part Data Transfer objects and Model objects.
/// </summary>
public static class PartMapper
{
    /// <summary>
    /// Maps the values of a Part Dto to a Model object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    public static Part DtoToModel(PartDto Dto)
    {
        return new Part
        (
            Dto.Id,
            Dto.PrintedBy,
            Dto.Number,
            Dto.Name,
            new ModelNumber(Dto.ModelCode)
        );
    }

    public static ProcessDto ModelToDto(Process Model)
    {
        throw new NotImplementedException();
    }
}