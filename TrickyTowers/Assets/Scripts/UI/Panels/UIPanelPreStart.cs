using System;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// UI panel displayed in pre-start game state. Shows countdown.
/// </summary>
public class UIPanelPreStart : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnCountdownEnd;

    // V A R I A B L E S

    [Header("ELEMENTS")]
    [SerializeField] private TMP_Text _countdownTxt;

    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;

    private YieldInstruction _timerTime;
    private float _coroutineTime;

    // M E T H O D S

    public void Setup()
    {
        _timerTime = new WaitForSeconds(
            _uiData.StartCountCycle - _uiData.StartCountAnimTime);

        Close();
    }

    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public new void Open(float p_fade = 0)
    {
        _countdownTxt.fontSize = 0;

        base.Open(p_fade);
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        _coroutineTime = 0;

        yield return new WaitForSecondsRealtime(_uiData.StartCountDelay);

        for (int i = 0; i < _uiData.StartCountStrings.Count; i++)
        {
            _countdownTxt.fontSize = 0;
            _countdownTxt.text = _uiData.StartCountStrings[i];
            _coroutineTime = 0;

            // Vibrate
            if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
                Vibration.VibratePop();

            // Lerp text appearing.
            while (_countdownTxt.fontSize < _uiData.StartCountSize)
            {
                _countdownTxt.fontSize = Mathf.Lerp(
                    0, _uiData.StartCountSize, _coroutineTime / _uiData.StartCountAnimTime);

                _coroutineTime += Time.unscaledDeltaTime;
                yield return null;
            }

            yield return _timerTime;
        }

        // Vibrate
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
            Vibration.VibratePeek();

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke();
    }
}
