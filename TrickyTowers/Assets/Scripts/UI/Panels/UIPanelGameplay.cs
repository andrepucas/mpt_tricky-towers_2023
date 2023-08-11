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

    public static event Action OnCountdownEnd;
    public static event Action OnPauseButton;

    // V A R I A B L E S

    [Header("CPU")]
    [SerializeField] private GameObject _cpuDisplay;

    [Header("NEXT BLOCK")]
    [SerializeField] private Image _nextBlockImage;

    [Header("LIVES")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Color _livesColorRed;
    [SerializeField] private Color _livesColorWhite;

    [Header("COUNTDOWN")]
    [SerializeField] private TMP_Text _countdownTxt;
    
    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;

    private float _animTime;
    private Coroutine _countdown;
    private YieldInstruction _timerTime;
    private float _coroutineTime;

    // G A M E   O B J E C T

    private void OnEnable()
    {
        BlockPoolSpawner.OnNextBlockPicked += UpdateNextBlock;
        ControllerPlayer.OnLivesUpdated += UpdateLivesCount;
        GameManager.OnDisplayCPU += ToggleCpuDisplay;
        FinishLine.OnFinishLineAction += ToggleFinishCounter;
    }

    private void OnDisable()
    {
        BlockPoolSpawner.OnNextBlockPicked -= UpdateNextBlock;
        ControllerPlayer.OnLivesUpdated -= UpdateLivesCount;
        GameManager.OnDisplayCPU -= ToggleCpuDisplay;
        FinishLine.OnFinishLineAction -= ToggleFinishCounter;
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
        _countdownTxt.text = "";
        base.Open(p_fade);
    }

    private void UpdateNextBlock(Sprite p_sprite, Color p_color)
    {
        _nextBlockImage.sprite = p_sprite;
        _nextBlockImage.color = p_color;
    }

    private void UpdateLivesCount(int p_lives)
    {
        if (p_lives >= 0)
        {
            if (_uiData.AnimateLivesLost)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateLivesCount(p_lives));
            }

            else _livesText.text = p_lives.ToString();
        }

        else _livesText.text = "-";
    }

    private void ToggleFinishCounter(bool p_toggle)
    {
        if (p_toggle) _countdown = StartCoroutine(CountdownToFinish());

        else
        {
            StopCoroutine(_countdown);
            _countdownTxt.text = "";
            _countdownTxt.fontSize = 0;
        }
    }

    private void ToggleCpuDisplay(bool p_toggle) => _cpuDisplay.SetActive(p_toggle);

    public void BtnPause() => OnPauseButton?.Invoke();

    // C O R O U T I N E S

    private IEnumerator AnimateLivesCount(int p_lives)
    {
        _animTime = 0;
        
        // Lerp to red.
        while (_livesImage.color != _livesColorRed)
        {
            _livesImage.color = Color.Lerp(
                _livesColorWhite, _livesColorRed, _animTime / _uiData.LivesColorLerpTime);

            _animTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _livesText.text = p_lives.ToString();

        _animTime = 0;
        
        // Lerp back to white.
        while (_livesImage.color != _livesColorWhite)
        {
            _livesImage.color = Color.Lerp(
                _livesColorRed, _livesColorWhite, _animTime / _uiData.LivesColorLerpTime);

            _animTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator CountdownToFinish()
    {

        _coroutineTime = 0;

        yield return new WaitForSecondsRealtime(_uiData.EndCountDelay);

        for (int i = 0; i < _uiData.EndCountStrings.Count; i++)
        {
            _countdownTxt.fontSize = 0;
            _countdownTxt.text = _uiData.EndCountStrings[i];
            _coroutineTime = 0;

            // Lerp text appearing.
            while (_countdownTxt.fontSize < _uiData.EndCountSize)
            {
                _countdownTxt.fontSize = Mathf.Lerp(
                    0, _uiData.EndCountSize, _coroutineTime / _uiData.EndCountAnimTime);

                _coroutineTime += Time.deltaTime;
                yield return null;
            }

            yield return _timerTime;
        }

        // Raise event when countdown ends.
        OnCountdownEnd?.Invoke();
    }
}
