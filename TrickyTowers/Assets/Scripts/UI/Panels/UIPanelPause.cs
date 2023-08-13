using System;

/// <summary>
/// UI panel displayed in the pause menu.
/// </summary>
public class UIPanelPause : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnResumeButton;
    public static event Action OnRetryButton;
    public static event Action OnQuitButton;

    // M E T H O D S

    public void Setup() => Close();
    public new void Close(float p_fade = 0) => base.Close(p_fade);
    public new void Open(float p_fade = 0) => base.Open(p_fade);

    public void BtnResume() => OnResumeButton?.Invoke();
    public void BtnRetry() => OnRetryButton?.Invoke();
    public void BtnQuit() => OnQuitButton?.Invoke();
}
