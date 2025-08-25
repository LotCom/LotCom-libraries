
using LotCom.Database.Enums;

namespace LotCom.Database.Extensions;

public static class AppPlatformExtensions
{
    public static string ToString(AppPlatform Value)
    {
        if (Value == AppPlatform.Cognex)
        {
            return "Cognex";
        }
        else
        {
            return "Windows";
        }
    }
}