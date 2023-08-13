using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI panel displayed during the gameplay loop.
/// </summary>
public class UIPanelGameplay : UIPanelAbstract
{
    // E V E N T S

    public static event Action<bool> OnCountdownEnd;
    public static event Action OnPauseButton;

    // V A R I A B L E S

    [Header("CPU")]
    [SerializeField] private GameObject _cpuDisplay;

    [Header("NEXT BLOCK")]
    [SerializeField] private Image _nextBlockImage;

    [Header("LIVES")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private Image _livesImage;

    [Header("COUNTDOWNS")]
    [SerializeField] private TMP_Text _winCountdownTxt;
    [SerializeField] private TMP_Text _loseCountdownTxt;
    [SerializeField] private FeedbackManager _feedback;
    
    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;

    private float _animTime;
    private Coroutine _winCountdown;
    private Coroutine _loseCountdown;
    private YieldInstruction _timerTime;
    private float _winCoroutineTime;
    private float _loseCoroutineTime;

    // G A M E   O B J E C T

    private void OnEnable()
    {
        BlockPoolSpawner.OnNextBlockPicked += UpdateNextBlock;
        ControllerPlayer.OnLivesUpdated += UpdateLivesCount;
        GameManager.OnDisplayCPU += ToggleCpuDisplay;
        FinishLine.OnWinAction += ToggleWinCounter;
        FinishLine.OnLoseAction += ToggleLoseCounter;
    }

    private void OnDisable()
    {
        BlockPoolSpawner.OnNextBlockPicked -= UpdateNextBlock;
        ControllerPlayer.OnLivesUpdated -= UpdateLivesCount;
        GameManager.OnDisplayCPU -= ToggleCpuDisplay;
        FinishLine.OnWinAction -= ToggleWinCounter;
        FinishLine.OnLoseAction -= ToggleLoseCounter;
    }

    // M E T H O D S

    public void Setup()
    {
        _timerTime = new WaitForSeconds(
            _uiData.EndCountCycle - _uiData.EndCountAnimTime);

        Close();
    }

    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public new void Open(float p_fade = 0)
    {
        _winCountdownTxt.text = "";
        _loseCountdownTxt.text = "";

        base.Open(p_fade);
    }

    private void UpdateNextBlock(Sprite p_sprite, Color p_color)
    {
        _nextBlockImage.sprite = p_sprite;
        _nextBlockImage.color = p_color;
    }

    private void UpdateLivesCount(int p_lives, bool p_reset)
    {
        if (p_lives >= 0)
        {
            if (_uiData.AnimateLivesLost && !p_reset)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateLivesCount(p_lives));
            }

            else _livesText.text = p_lives.ToString();
        }

        else _livesText.text = "-";
    }

    private void ToggleWinCounter(bool p_toggle)
    {
        if (p_toggle) _winCountdown = StartCoroutine(CountdownToWin());

        else
        {
            StopCoroutine(_winCountdown);
            _winCountdownTxt.text = "";
            _winCountdownTxt.fontSize = 0;
        }
    }

    private void ToggleLoseCounter(bool p_toggle)
    {
        if (p_toggle) _loseCountdown = StartCoroutine(CountdownToLose());

        else
        {
            StopCoroutine(_loseCountdown);
            _loseCountdownTxt.text = "";
            _loseCountdownTxt.fontSize = 0;
        }
    }

    private void ToggleCpuDisplay(bool p_toggle) => _cpuDisplay.SetActive(p_toggle);

    public void BtnPause() => OnPauseButton?.Invoke();

    // C O R O U T I N E S

    private IEnumerator AnimateLivesCount(int p_lives)
    {
        _animTime = 0;

        // Lerp to white.
        while (_livesImage.color != _uiData.ColorWhite)
        {
            _livesImage.color = Color.Lerp(_uiData.ColorRed, _uiData.ColorWhite, 
                _animTime / _uiData.LivesColorLerpTime);

            _animTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _livesText.text = p_lives.ToString();

        _animTime = 0;

        // Lerp to red.
        while (_livesImage.color != _uiData.ColorRed)
        {
            _livesImage.color = Color.Lerp(_uiData.ColorWhite, _uiData.ColorRed, 
                _animTime / _uiData.LivesColorLerpTime);

            _animTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator CountdownToWin()
    {
        _winCoroutineTime = 0;

        yield return new WaitForSecondsRealtime(_uiData.EndCountDelay);

        for (int i = 0; i < _uiData.EndCountStrings.Count; i++)
        {
            _winCountdownTxt.fontSize = 0;
            _winCountdownTxt.text = _uiData.EndCountStrings[i];
            _winCoroutineTime = 0;

            if (i < _uiData.StartCountStrings.Count - 1) _feedback.OnUICountdownPop();
            else _feedback.OnUICountdownPeek();

            // Lerp text appearing.
            while (_winCountdownTxt.fontSize < _uiData.EndCountWinSize)
            {
                _winCountdownTxt.fontSize = Mathf.Lerp(
                    0, _uiData.EndCountWinSize, _winCoroutineTime / _uiData.EndCountAnimTime);

                _winCoroutineTime += Time.deltaTime;
                yield return null;
            }

            yield return _timerTime;
        }

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke(true);
    }

    private IEnumerator CountdownToLose()
    {
        _loseCoroutineTime = 0;

        yield return new WaitForSecondsRealtime(_uiData.EndCountDelay);

        for (int i = 0; i < _uiData.EndCountStrings.Count; i++)
        {
            _loseCountdownTxt.fontSize = 0;
            _loseCountdownTxt.text = _uiData.EndCountStrings[i];
            _loseCoroutineTime = 0;

            if (i < _uiData.StartCountStrings.Count - 1) _feedback.OnUICountdownPop();
            else _feedback.OnUICountdownPeek();

            // Lerp text appearing.
            while (_loseCountdownTxt.fontSize < _uiData.EndCountLoseSize)
            {
                _loseCountdownTxt.fontSize = Mathf.Lerp(
                    0, _uiData.EndCountLoseSize, _loseCoroutineTime / _uiData.EndCountAnimTime);

                _loseCoroutineTime += Time.deltaTime;
                yield return null;
            }

            yield return _timerTime;
        }

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke(false);
    }
}
