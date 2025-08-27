using LotCom.Database.Auth;
using LotCom.Database.Mappers;

namespace LotCom.Database.Balancing;

public interface IBalancer<TEntity, TDto, TModel>
{
    /// <summary>
    /// Uses chunking to convert an IEnumerable TDto of any size, returning a single IEnumerable TModel.
    /// </summary>
    /// <param name="Input"></param>
    /// <returns></returns>
    Task<IEnumerable<TModel>> ConvertUsingChunking(IEnumerable<TDto> Input, IMapper<TModel, TEntity, TDto> Mapper, HttpClient Client, UserAgent Agent);
}