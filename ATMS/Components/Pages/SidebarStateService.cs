// SidebarStateService.cs
public class SidebarStateService
{
    public bool IsCollapsed { get; private set; }

    public event Action? OnChange;

    public void Toggle()
    {
        IsCollapsed = !IsCollapsed;
        OnChange?.Invoke();
    }

    public void SetCollapsed(bool collapsed)
    {
        IsCollapsed = collapsed;
        OnChange?.Invoke();
    }
}