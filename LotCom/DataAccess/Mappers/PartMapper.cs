using LotCom.DataAccess.Models;
using LotCom.DataAccess.Services;
using LotCom.Exceptions;
using LotCom.Types;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Part Data Transfer objects and Model objects.
/// </summary>
public static class PartMapper
{
    /// <summary>
    /// Maps the values of a Part DTO to a Model object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    public static async Task<Part> DtoToModel(PartDto Dto)
    {
        Process? ModelProcess = await ProcessService.Get(Dto.PrintedBy);
        if (ModelProcess is null)
        {
            throw new DatabaseException("Could not retrieve the Part's parent Process (printed by).");
        }
        return new Part
        (
            ModelProcess.FullName,
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