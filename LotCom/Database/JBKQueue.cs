using LotCom.Enums;

namespace LotCom.Database;

public partial class JBKQueue() : SerialQueue
(
    QueuePath: "\\\\144.133.122.1\\Lot Control Management\\Database\\process_control\\serial_queues\\jbk_queues.json",
    Mode: SerializationMode.JBK,
    Limit: 999
)
{

}