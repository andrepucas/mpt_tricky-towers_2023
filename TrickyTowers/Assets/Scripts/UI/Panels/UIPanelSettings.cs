using System;
using UnityEngine;
using TMPro;

/// <summary>
/// UI panel displayed in the settings menu.
/// </summary>
public class UIPanelSettings : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnBackButton;

    // V A R I A B L E S

    [Header("ELEMENTS")]
    [SerializeField] private TMP_Text _fpsTxt;
    [SerializeField] private GameObject _showFpsToggle;
    [SerializeField] private GameObject _vibrationToggle;

    private int _fpsIndex;
    private int _showFpsIndex;
    private int _vibrationIndex;

    // M E T H O D S

    public void Setup() => Close();
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public new void Open(float p_fade = 0)
    {
        _fpsIndex = PlayerPrefs.GetInt(_savedData.FpsPrefName, _savedData.FpsDefault);
        UpdateFPS();

        _showFpsIndex = PlayerPrefs.GetInt(_savedData.ShowFpsPrefName, _savedData.ShowFPSDefault);
        UpdateShowFPS();

        _vibrationIndex = PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault);
        UpdateVibration();

        base.Open(p_fade);
    }

    private void UpdateFPS()
    {
        _fpsTxt.text = _savedData.FpsOptions[_fpsIndex].ToString();
        PlayerPrefs.SetInt(_savedData.FpsPrefName, _fpsIndex);
    }

    private void UpdateShowFPS()
    {
        if (_showFpsIndex == 0) _showFpsToggle.SetActive(false);
        else _showFpsToggle.SetActive(true);

        PlayerPrefs.SetInt(_savedData.ShowFpsPrefName, _showFpsIndex);
    }

    private void UpdateVibration()
    {
        if (_vibrationIndex == 0) _vibrationToggle.SetActive(false);
        else _vibrationToggle.SetActive(true);

        PlayerPrefs.SetInt(_savedData.VibrationPrefName, _vibrationIndex);
    }

    public void BtnFPS()
    {
        _fpsIndex++;

        if (_fpsIndex >= _savedData.FpsOptions.Count) _fpsIndex = 0;

        UpdateFPS();

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePop();
    }

    public void BtnShowFPS()
    {
        if (_showFpsIndex == 0) _showFpsIndex = 1;
        else _showFpsIndex = 0;

        UpdateShowFPS();

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePop();
    }

    public void BtnVibration()
    {
        if (_vibrationIndex == 0) _vibrationIndex = 1;
        else _vibrationIndex = 0;

        UpdateVibration();

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePop();
    }

    public void BtnBack()
    {
        OnBackButton?.Invoke();

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePop();
    }
}