using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI panel displayed in game-over, win and loss.
/// </summary>
public class UIPanelEnd : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnRetryButton;
    public static event Action OnQuitButton;

    // V A R I A B L E S

    [SerializeField] private Image _titleImg;
    [SerializeField] private UserInterfaceDataSO _uiData;

    // M E T H O D S

    public void Setup() => Close();
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public void OpenWin(float p_fade = 0)
    {
        _titleImg.sprite = _uiData.EndSpriteWin;
        Open(p_fade);
    }

    public void OpenLose(float p_fade = 0)
    {
        _titleImg.sprite = _uiData.EndSpriteLose;
        Open(p_fade);
    }

    public void BtnRetry() => OnRetryButton?.Invoke();
    public void BtnQuit() => OnQuitButton?.Invoke();
}
