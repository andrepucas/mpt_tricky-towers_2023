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

    [SerializeField] private TMP_Text _countdownTxt;
    [SerializeField] private UserInterfaceDataSO _uiData;

    // M E T H O D S

    public new void Open(float p_fade = 0)
    {
        _countdownTxt.fontSize = 0;

        base.Open(p_fade);
        StartCoroutine(Countdown());
    }

    public new void Close(float p_fade = 0) => base.Close(p_fade);

    private IEnumerator Countdown()
    {
        YieldInstruction m_timerTime = new WaitForSeconds(
            _uiData.StartCountCycle - _uiData.StartCountAnimTime);

        float m_elapsedTime = 0;

        yield return new WaitForSecondsRealtime(_uiData.StartCountDelay);

        for (int i = 0; i < _uiData.StartCountStrings.Count; i++)
        {
            _countdownTxt.fontSize = 0;
            _countdownTxt.text = _uiData.StartCountStrings[i];
            m_elapsedTime = 0;

            // Lerp text appearing.
            while (_countdownTxt.fontSize < _uiData.StartCountSize)
            {
                _countdownTxt.fontSize = Mathf.Lerp(
                    0, _uiData.StartCountSize, m_elapsedTime / _uiData.StartCountAnimTime);

                m_elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            yield return m_timerTime;
        }

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke();
    }
}
