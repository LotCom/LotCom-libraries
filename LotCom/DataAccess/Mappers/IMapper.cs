using LotCom.DataAccess.Auth;

namespace LotCom.DataAccess.Mappers;

/// <summary>
/// Provides value mapping between Model, Entity, and DTO objects for LotCom native types.
/// </summary>
/// <remarks>
/// TModel should be the Model class, TEntity the Entity class, and TDto the DTO class of the native type.
/// </remarks>
/// <typeparam name="TModel">A Model class for T, the LotCom.Core native type.</typeparam>
/// <typeparam name="TEntity">An Entity class for T, the LotCom.Core native type.</typeparam>
/// <typeparam name="TDto">A DTO class for T, the LotCom.Core native type.</typeparam>
public interface IMapper<TModel, TEntity, TDto>
{
    /// <summary>
    /// Maps a DTO's values to a new Model object.
    /// </summary>
    /// <remarks>
    /// Must be async and return Task promising TModel, as most Model creations require Db interactions.
    /// </remarks>
    /// <param name="Dto"></param>
    /// <param name="Agent"></param>
    /// <returns></returns>
    Task<TModel> DtoToModel(TDto Dto, UserAgent Agent);
    
    /// <summary>
    /// Maps a DTO's values to a new Entity object.
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    TEntity DtoToEntity(TDto Dto);
    
    /// <summary>
    /// Maps a DTO's values to a new DTO object (essentially shallow-copying the object).
    /// </summary>
    /// <param name="Dto"></param>
    /// <returns></returns>
    TDto DtoToDto(TDto Dto);
    
    /// <summary>
    /// Maps a Model's values to a new DTO object.
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    TDto ModelToDto(TModel Model);
    
    /// <summary>
    /// Maps a Model's values to a new Model object (essentially shallow-copying the object).
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    TModel ModelToModel(TModel Model);
    
    /// <summary>
    /// Maps an Entity's values to a new DTO object.
    /// </summary>
    /// <remarks>
    /// Omits any sensitive Entity data.
    /// </remarks>
    /// <param name="Entity"></param>
    /// <returns></returns>
    TDto EntityToDto(TEntity Entity);
    
    /// <summary>
    /// Maps an Entity's values (Source) to another Entity object (Recipient), essentially shallow-copying the object.
    /// </summary>
    /// <param name="Recipient"></param>
    /// <param name="Source"></param>
    /// <returns></returns>
    void UpdateEntity(TEntity Recipient, TEntity Source);
}