using System;

/// <summary>
/// UI panel displayed in the main menu.
/// </summary>
public class UIPanelMainMenu : UIPanelAbstract
{
    // E V E N T S

    public static event Action<bool> OnPlayButtons;

    // M E T H O D S

    public new void Open(float p_fade = 0) => base.Open(p_fade);
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public void BtnCasual() => OnPlayButtons?.Invoke(false);
    public void BtnVersus() => OnPlayButtons?.Invoke(true);
}
