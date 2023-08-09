using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI panel displayed during the gameplay loop.
/// </summary>
public class UIPanelGameplay : UIPanelAbstract
{
    // E V E N T S

    public static event Action OnPauseButton;

    // V A R I A B L E S

    [SerializeField] private Image _nextBlockImage;

    // G A M E   O B J E C T

    private void OnEnable() => BlockPoolSpawner.OnNextBlockPicked += UpdateNextBlock;
    private void OnDisable() => BlockPoolSpawner.OnNextBlockPicked -= UpdateNextBlock;

    // M E T H O D S

    public new void Open(float p_fade = 0) => base.Open(p_fade);
    public new void Close(float p_fade = 0) => base.Close(p_fade);

    public void BtnPause() => OnPauseButton?.Invoke();

    private void UpdateNextBlock(Sprite p_sprite, Color p_color)
    {
        _nextBlockImage.sprite = p_sprite;
        _nextBlockImage.color = p_color;
    }
}
