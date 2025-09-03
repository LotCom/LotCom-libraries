using LotCom.Core.Models;

namespace LotCom.Database.Caching;

/// <summary>
/// Acts as a caching mechanism that stores Database information from executed Process queries.
/// </summary>
public class ProcessCache : ICache<Process>
{
    public IEnumerable<Process> CacheItems
    {
        get;
        set;
    }

    /// <summary>
    /// Creates a new ProcessCache.
    /// </summary>
    public ProcessCache()
    {
        Console.WriteLine("Creating a new Cache for Processes...");
        CacheItems = [];
    }

    public Process? Get(int id)
    {
        return CacheItems
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }

    public void Add(Process processToCache)
    {
        if (Get(processToCache.Id) is null)
        {
            CacheItems = CacheItems.Append(processToCache);
        }
    }

    public void AddRange(IEnumerable<Process> rangeToCache)
    {
        foreach (Process _process in rangeToCache)
        {
            Add(_process);
        }
    }

    public bool Remove(Process processToRemove)
    {
        if (Get(processToRemove.Id) is null)
        {
            return false;
        }
        else
        {
            CacheItems = CacheItems
                .Where(x => x.Id != processToRemove.Id);
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