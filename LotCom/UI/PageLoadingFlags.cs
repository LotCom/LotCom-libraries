using CommunityToolkit.Mvvm.ComponentModel;

namespace LotCom.UI;

/// <summary>
/// Flags that indicate how the Page is loading and/or has loaded its data.
/// </summary>
public partial class PageLoadingFlags : ObservableObject
{
    [ObservableProperty]
    public virtual partial bool IsLoading { get; set; } = false;

    [ObservableProperty]
    public virtual partial bool IsComplete { get; set; } = false;

    [ObservableProperty]
    public virtual partial bool IsSuccess { get; set; } = false;

    [ObservableProperty]
    public virtual partial bool IsFaulted { get; set; } = false;

    /// <summary>
    /// Sets all Page flags to false.
    /// </summary>
    public virtual void Reset()
    {
        IsLoading = false;
        IsSuccess = false;
        IsFaulted = false;
        IsComplete = false;
    }

    /// <summary>
    /// Toggles the Page flags to indicate that a load event has started.
    /// </summary>
    public virtual void Start()
    {
        IsLoading = true;
        IsSuccess = false;
        IsFaulted = false;
        IsComplete = false;
    }

    /// <summary>
    /// Toggles the Page flags to indicate a load event has ended successfully.
    /// </summary>
    public virtual void Success()
    {
        IsLoading = false;
        IsSuccess = true;
        IsFaulted = false;
        IsComplete = true;
    }

    /// <summary>
    /// Toggles the Page flags to indicate a load event has ended unsuccessfully.
    /// </summary>
    public virtual void Failure()
    {
        IsLoading = false;
        IsSuccess = false;
        IsFaulted = true;
        IsComplete = true;
    }
}