using System;

/// <summary>
/// UI panel displayed during the gameplay loop.
/// </summary>
public class UIPanelGameplay : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnPauseButton;

    // M E T H O D S

    public new void Open(float p_fade = 0) => base.Open(p_fade);
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public void BtnPause() => OnPauseButton?.Invoke();
}
