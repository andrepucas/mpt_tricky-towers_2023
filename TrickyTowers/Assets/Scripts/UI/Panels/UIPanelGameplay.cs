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

    public static event Action OnPauseButton;

    // V A R I A B L E S

    [Header("NEXT BLOCK")]
    [SerializeField] private Image _nextBlockImage;
    
    [Header("LIVES")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Color _livesColorRed;
    [SerializeField] private Color _livesColorWhite;
    
    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;

    private float _animTime;

    // G A M E   O B J E C T

    private void OnEnable()
    {
        BlockPoolSpawner.OnNextBlockPicked += UpdateNextBlock;
        Controller.OnLivesUpdated += UpdateLivesCount;
    }

    private void OnDisable()
    {
        BlockPoolSpawner.OnNextBlockPicked -= UpdateNextBlock;
        Controller.OnLivesUpdated -= UpdateLivesCount;
    }

    // M E T H O D S

    public new void Open(float p_fade = 0) => base.Open(p_fade);
    public new void Close(float p_fade = 0) => base.Close(p_fade);

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

    public void BtnPause() => OnPauseButton?.Invoke();
}
