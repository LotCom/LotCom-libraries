using LotCom.Core.Models;
using LotCom.Database.Auth;
using LotCom.Database.Entities;
using LotCom.Database.Mappers;
using LotCom.Database.Transfer;

namespace LotCom.Database.Balancing;

public class ScanBalancer : IBalancer<ScanEntity, ScanDto, Scan>
{
    /// <summary>
    /// The size of chunks to use in Scan chunking processes.
    /// </summary>
    private const int ChunkSize = 5;

    public async Task<IEnumerable<Scan>> ConvertUsingChunking(IEnumerable<ScanDto> Input, IMapper<Scan, ScanEntity, ScanDto> Mapper, UserAgent Agent)
    { 
        IEnumerable<Scan> Scans = [];
        // chunk the input
        foreach (var _chunk in Input.Chunk(ChunkSize))
        {
            Console.WriteLine($"Processing a chunk of {ChunkSize} scans...");
            // parse each chunk item asynchronously
            IEnumerable<Scan> _chunkParsed = await Task.WhenAll
            (
                _chunk.Select(async x => await Mapper.DtoToModel(x, Agent))
            );
            // add all of the scans to the end of the main enumerable
            Scans = Scans.Concat(_chunkParsed);
        }
        return Scans;
    }
}