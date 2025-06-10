using LotCom.Enums;

namespace LotCom.Database;

public partial class LotQueue() : SerialQueue
(
    QueuePath: "\\\\144.133.122.1\\Lot Control Management\\Database\\process_control\\serial_queues\\lot_queues.json",
    Mode: SerializationMode.Lot,
    Limit: 999999999
)
{

}