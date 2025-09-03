namespace LotCom.Database.Caching;

/// <summary>
/// Acts as a caching mechanism that stores Database information from executed queries.
/// </summary>
public interface ICache<TModel>
{
    /// <summary>
    /// The Models currently held by the ICache.
    /// </summary>
    IEnumerable<TModel> CacheItems { get; set; }

    /// <summary>
    /// Returns a cached Model, or null, if the passed Model Id is not cached.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TModel? Get(int id);

    /// <summary>
    /// Checks if the Model exists in the ICache and adds it if not.
    /// </summary>
    /// <param name="modelToCache"></param>
    void Add(TModel modelToCache);

    /// <summary>
    /// Adds all non-cached Models in rangeToCache to the ICache.
    /// </summary>
    /// <param name="rangeToCache"></param>
    void AddRange(IEnumerable<TModel> rangeToCache);

    /// <summary>
    /// Attempts to remove modelToRemove from the ICache.
    /// </summary>
    /// <param name="modelToRemove"></param>
    /// <returns>true if the Model was in the ICache; false if not.</returns>
    bool Remove(TModel modelToRemove);

    /// <summary>
    /// Attempts to remove a Model with Id idToRemove from the ICache.
    /// </summary>
    /// <param name="idToRemove"></param>
    /// <returns>true if a Model with Id idToRemove was in the ICache; false if not.</returns>
    bool Remove(int idToRemove);

    /// <summary>
    /// Clears all of the Models from the ICache.
    /// </summary>
    void Clear();
}