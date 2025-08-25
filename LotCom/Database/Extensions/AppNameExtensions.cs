using LotCom.Database.Enums;

namespace LotCom.Database.Extensions;

public static class AppNameExtensions
{
    public static string ToString(AppName Value)
    {
        if (Value == AppName.Scanner)
        {
            return "Scanner";
        }
        else if (Value == AppName.Printer)
        {
            return "Printer";
        }
        else if (Value == AppName.Watcher)
        {
            return "Watcher";
        }
        else if (Value == AppName.Client)
        {
            return "Client";
        }
        else
        {
            return "API";
        }
    }
}