using LotCom.DataAccess.Enums;

namespace LotCom.DataAccess;

public static class UserAgentFactory
{
    /// <summary>
    /// Generates a new UA object for the LotCom Printer application at the specified version number.
    /// </summary>
    /// <param name="Major"></param>
    /// <param name="Minor"></param>
    /// <param name="Patch"></param>
    /// <returns></returns>
    public static UserAgent CreatePrinterAgent(int Major, int Minor, int Patch)
    {
        return new UserAgent
        (
            AppName.Printer,
            $"{Major}.{Minor}.{Patch}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }

    /// <summary>
    /// Generates a new UA object for the LotCom Printer application at the specified version number.
    /// </summary>
    /// <param name="Version"></param>
    /// <returns></returns>
    public static UserAgent CreatePrinterAgent(string Version)
    {
        string[] SplitVersion = Version.Split('.');
        return new UserAgent
        (
            AppName.Printer,
            $"{SplitVersion[0]}.{SplitVersion[0]}.{SplitVersion[0]}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }

    /// <summary>
    /// Generates a new UA object for the LotCom Watcher application at the specified version number.
    /// </summary>
    /// <param name="Major"></param>
    /// <param name="Minor"></param>
    /// <param name="Patch"></param>
    /// <returns></returns>
    public static UserAgent CreateWatcherAgent(int Major, int Minor, int Patch)
    {
        return new UserAgent
        (
            AppName.Watcher,
            $"{Major}.{Minor}.{Patch}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }

    /// <summary>
    /// Generates a new UA object for the LotCom Watcher application at the specified version number.
    /// </summary>
    /// <param name="Version"></param>
    /// <returns></returns>
    public static UserAgent CreateWatcherAgent(string Version)
    {
        string[] SplitVersion = Version.Split('.');
        return new UserAgent
        (
            AppName.Watcher,
            $"{SplitVersion[0]}.{SplitVersion[0]}.{SplitVersion[0]}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }

    /// <summary>
    /// Generates a new UA object for the LotCom Client application at the specified version number.
    /// </summary>
    /// <param name="Major"></param>
    /// <param name="Minor"></param>
    /// <param name="Patch"></param>
    /// <returns></returns>
    public static UserAgent CreateClientAgent(int Major, int Minor, int Patch)
    {
        return new UserAgent
        (
            AppName.Client,
            $"{Major}.{Minor}.{Patch}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }

    /// <summary>
    /// Generates a new UA object for the LotCom Client application at the specified version number.
    /// </summary>
    /// <param name="Version"></param>
    /// <returns></returns>
    public static UserAgent CreateClientAgent(string Version)
    {
        string[] SplitVersion = Version.Split('.');
        return new UserAgent
        (
            AppName.Client,
            $"{SplitVersion[0]}.{SplitVersion[0]}.{SplitVersion[0]}",
            AppPlatform.Windows,
            AppEnvironment.DOTNET
        );
    }
    
    /// <summary>
    /// Generates a new UA object for a LotCom Scanner.
    /// </summary>
    /// <returns></returns>
    public static UserAgent CreateScannerAgent()
    {
        return new UserAgent
        (
            AppName.Scanner,
            "1.0.0",
            AppPlatform.Cognex,
            AppEnvironment.JavaScript
        );
    }
}