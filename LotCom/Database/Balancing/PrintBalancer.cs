using LotCom.Core.Models;
using LotCom.Database.Auth;
using LotCom.Database.Entities;
using LotCom.Database.Mappers;
using LotCom.Database.Transfer;

namespace LotCom.Database.Balancing;

public class PrintBalancer : IBalancer<PrintEntity, PrintDto, Print>
{
    /// <summary>
    /// The size of chunks to use in Print chunking processes.
    /// </summary>
    private const int ChunkSize = 5;

    public async Task<IEnumerable<Print>> ConvertUsingChunking(IEnumerable<PrintDto> Input, IMapper<Print, PrintEntity, PrintDto> Mapper, UserAgent Agent)
    { 
        IEnumerable<Print> Prints = [];
        // chunk the input
        foreach (var _chunk in Input.Chunk(ChunkSize))
        {
            Console.WriteLine($"Processing a chunk of {ChunkSize} prints...");
            // parse each chunk item asynchronously
            IEnumerable<Print> _chunkParsed = await Task.WhenAll
            (
                _chunk.Select(async x => await Mapper.DtoToModel(x, Agent))
            );
            // add all of the prints to the end of the main enumerable
            Prints = Prints.Concat(_chunkParsed);
        }
        return Prints;
    }
}