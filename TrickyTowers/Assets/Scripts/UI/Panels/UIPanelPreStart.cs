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
    [SerializeField] private GameDataSO _data;

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
            _data.CountdownDelay - _data.CountdownAnimTime);

        float m_elapsedTime = 0;

        for (int i = 0; i < _data.CountdownStrings.Count; i++)
        {
            _countdownTxt.fontSize = 0;
            _countdownTxt.text = _data.CountdownStrings[i];
            m_elapsedTime = 0;

            // Lerp text appearing.
            while (_countdownTxt.fontSize < _data.CountdownSize)
            {
                _countdownTxt.fontSize = Mathf.Lerp(
                    0, _data.CountdownSize, m_elapsedTime / _data.CountdownAnimTime);

                m_elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (i < _data.CountdownStrings.Count - 1) yield return m_timerTime;
        }

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke();
    }
}
