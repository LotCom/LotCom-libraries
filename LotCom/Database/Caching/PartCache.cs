using LotCom.Core.Models;

namespace LotCom.Database.Caching;

/// <summary>
/// Acts as a caching mechanism that stores Database information from executed Part queries.
/// </summary>
public class PartCache : ICache<Part>
{
    public IEnumerable<Part> CacheItems
    {
        get;
        set;
    }

    /// <summary>
    /// Creates a new PartCache.
    /// </summary>
    public PartCache()
    {
        Console.WriteLine("Creating a new Cache for Parts...");
        CacheItems = [];
    }

    public Part? Get(int id)
    {
        return CacheItems
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }

    public void Add(Part partToCache)
    {
        if (Get(partToCache.Id) is null)
        {
            CacheItems = CacheItems.Append(partToCache);
        }
    }

    public void AddRange(IEnumerable<Part> rangeToCache)
    {
        foreach (Part _part in rangeToCache)
        {
            Add(_part);
        }
    }

    public bool Remove(Part partToRemove)
    {
        if (Get(partToRemove.Id) is null)
        {
            return false;
        }
        else
        {
            CacheItems = CacheItems
                .Where(x => x.Id != partToRemove.Id);
            return true;
        }
    }

    public bool Remove(int idToRemove)
    {
        if (Get(idToRemove) is null)
        {
            return false;
        }
        else
        {
            CacheItems = CacheItems
                .Where(x => x.Id != idToRemove);
            return true;
        }
    }

    public void Clear()
    {
        CacheItems = [];
    }
}