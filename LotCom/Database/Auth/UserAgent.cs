using LotCom.Database.Enums;

namespace LotCom.Database.Auth;

/// <summary>
/// A LotCom User-Agent configuration to apply headers to API calls.
/// </summary>
/// <param name="AppName"></param>
/// <param name="AppVersion"></param>
/// <param name="Platform"></param>
/// <param name="Environment"></param>
public class UserAgent(AppName AppName, string AppVersion, AppPlatform? Platform = AppPlatform.Windows, AppEnvironment? Environment = AppEnvironment.DOTNET)
{
    public readonly AppName AppName = AppName;
    public readonly string AppVersion = AppVersion;
    public readonly AppPlatform Platform = (AppPlatform)Platform!;
    public readonly AppEnvironment Environment = (AppEnvironment)Environment!;
}