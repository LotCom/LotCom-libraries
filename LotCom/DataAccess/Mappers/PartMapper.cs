using LotCom.DataAccess.Models;
using LotCom.Types;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Part Data Transfer objects and Model objects.
/// </summary>
public static class PartMapper
{
    /// <summary>
    /// Maps the values of a Part Dao to a Model object.
    /// </summary>
    /// <param name="Dao"></param>
    /// <returns></returns>
    public static Part DaoToModel(PartDao Dao)
    {
        return new Part
        (
            Dao.Id,
            Dao.PrintedBy,
            Dao.Number,
            Dao.Name,
            new ModelNumber(Dao.ModelCode)
        );
    }

    public static ProcessDao ModelToDao(Process Model)
    {
        throw new NotImplementedException();
    }
}