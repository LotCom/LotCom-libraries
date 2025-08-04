using LotCom.DataAccess.Enums;

namespace LotCom.DataAccess.Extensions;

public static class AppEnvironmentExtensions
{
    public static string ToString(AppEnvironment Value)
    {
        if (Value == AppEnvironment.JavaScript)
        {
            return "JavaScript";
        }
        else
        {
            return ".NET";
        };
    }
}