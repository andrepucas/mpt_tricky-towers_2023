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

    [Header("ELEMENTS")]
    [SerializeField] private Image _titleImg;
    [SerializeField] private GameObject _cpuDisplay;
    [SerializeField] private Image _backgroundImg;

    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;

    // G A M E   O B J E C T

    private void OnEnable() => GameManager.OnDisplayCPU += ToggleCpuDisplay;
    private void OnDisable() => GameManager.OnDisplayCPU -= ToggleCpuDisplay;

    // M E T H O D S

    public void Setup() => Close();
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public void OpenWin(float p_fade = 0)
    {
        _titleImg.sprite = _uiData.EndSpriteWin;
        _titleImg.color = _uiData.ColorGold;
        _backgroundImg.color = _uiData.ColorGold;
        Open(p_fade);

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePeek();
    }

    public void OpenLose(float p_fade = 0)
    {
        _titleImg.sprite = _uiData.EndSpriteLose;
        _titleImg.color = _uiData.ColorWhite;
        _backgroundImg.color = _uiData.ColorRed;
        Open(p_fade);

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibrateNope();
    }

    private void ToggleCpuDisplay(bool p_toggle) => _cpuDisplay.SetActive(p_toggle);

    public void BtnRetry() => OnRetryButton?.Invoke();
    public void BtnQuit() => OnQuitButton?.Invoke();
}
