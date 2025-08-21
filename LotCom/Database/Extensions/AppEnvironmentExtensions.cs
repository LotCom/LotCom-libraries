using LotCom.Database.Enums;

namespace LotCom.Database.Extensions;

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