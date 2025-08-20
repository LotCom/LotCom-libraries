using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.UI;

/// <summary>
/// Extension of the PageLoadingFlags class that provides compound flags for Process and Part Loading.
/// Base class properties indicate the status of the compound Process and Part loading.
/// </summary>
public partial class CompoundPageLoadingFlags : PageLoadingFlags
{
    [ObservableProperty]
    public partial bool IsProcessLoading { get; set; } = false;

    [ObservableProperty]
    public partial bool IsProcessSuccess { get; set; } = false;

    [ObservableProperty]
    public partial bool IsProcessFaulted { get; set; } = false;

    [ObservableProperty]
    public partial bool IsProcessComplete { get; set; } = false;

    [ObservableProperty]
    public partial bool IsPartLoading { get; set; } = false;

    [ObservableProperty]
    public partial bool IsPartSuccess { get; set; } = false;

    [ObservableProperty]
    public partial bool IsPartFaulted { get; set; } = false;

    [ObservableProperty]
    public partial bool IsPartComplete { get; set; } = false;

    /// <summary>
    /// Indicates the start of a Process Loading event.
    /// </summary>
    public void StartProcesses()
    {
        base.Start();
        IsProcessLoading = true;
        IsProcessFaulted = false;
        IsProcessSuccess = false;
        IsProcessComplete = false;
    }

    /// <summary>
    /// Indicates that a Process Loading event failed.
    /// </summary>
    public void FailureProcesses()
    {
        base.Failure();
        IsProcessLoading = false;
        IsProcessFaulted = true;
        IsProcessSuccess = false;
        IsProcessComplete = true;
    }

    /// <summary>
    /// Indicates that a Process Loading event succeeded.
    /// </summary>
    public void SuccessProcesses()
    {
        base.Success();
        IsProcessLoading = false;
        IsProcessFaulted = false;
        IsProcessSuccess = true;
        IsProcessComplete = true;
    }

    /// <summary>
    /// Resets all Process loading flags.
    /// </summary>
    public void ResetProcesses()
    {
        base.Reset();
        IsProcessLoading = false;
        IsProcessFaulted = false;
        IsProcessSuccess = false;
        IsProcessComplete = false;
    }

    /// <summary>
    /// Indicates the start of a Part Loading event.
    /// </summary>
    public void StartParts()
    {
        base.Start();
        IsPartLoading = true;
        IsPartFaulted = false;
        IsPartSuccess = false;
        IsPartComplete = false;
    }

    /// <summary>
    /// Indicates that a Part Loading event failed.
    /// </summary>
    public void FailureParts()
    {
        base.Failure();
        IsPartLoading = false;
        IsPartFaulted = true;
        IsPartSuccess = false;
        IsPartComplete = true;
    }

    /// <summary>
    /// Indicates that a Part Loading event succeeded.
    /// </summary>
    public void SuccessParts()
    {
        base.Success();
        IsPartLoading = false;
        IsPartFaulted = false;
        IsPartSuccess = true;
        IsPartComplete = true;
    }

    /// <summary>
    /// Resets all Part loading flags.
    /// </summary>
    public void ResetParts()
    {
        base.Reset();
        IsPartLoading = false;
        IsPartFaulted = false;
        IsPartSuccess = false;
        IsPartComplete = false;
    }
}