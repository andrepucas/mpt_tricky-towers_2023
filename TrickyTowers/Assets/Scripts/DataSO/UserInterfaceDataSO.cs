using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of UI related variables.
/// </summary>
[CreateAssetMenu(fileName = "UI Data", menuName = "Data/UI Data")]
public class UserInterfaceDataSO : ScriptableObject
{
    [Header("DEBUG")]
    [SerializeField] private bool _displayFPS;

    public bool DisplayFPS => _displayFPS;

    [Header("GENERAL")]
    [Tooltip("Time (s) to wait before revealing main menu. 0 might lead to laggy opening animation.")]
    [SerializeField][Range(0, 2)] private float _setupDelay;
    [Tooltip("Transition time for main menu UI panel to open.")]
    [SerializeField][Range(0.5f, 5)] private float _revealFade;
    [Tooltip("Transition time (s) for UI panels to open or close.")]
    [SerializeField][Range(0, 2)] private float _panelFade;

    public float SetupDelay => _setupDelay;
    public float RevealFade => _revealFade;
    public float PanelFade => _panelFade;

    [Header("PRE-START")]
    [SerializeField] private bool _displayCountdown = true;
    [Tooltip("Text to be displayed in the countdown. Default: 3, 2, 1, GO!")]
    [SerializeField] private string[] _countdownStrings = {"3", "2", "1", "GO!"};
    [Tooltip("Time (s) before the countdown starts.")]
    [SerializeField][Range(0, 10)] private float _countdownDelay = 1;
    [Tooltip("Time (s) it takes to cycle through each strings. Default = 1.")]
    [SerializeField][Range(0, 3)] private float _countdownCycle = 1;
    [Tooltip("Time (s) each string takes to scale up. Default = 0.5s")]
    [SerializeField][Range(0, 3)] private float _countdownAnimTime = 0.5f;
    [Tooltip("Target font size.")]
    [SerializeField] private float _countdownSize;

    public bool DisplayCountdown => _displayCountdown;
    public IReadOnlyList<string> CountdownStrings => _countdownStrings;
    public float CountdownDelay => _countdownDelay;
    public float CountdownCycle => _countdownCycle;
    public float CountdownAnimTime => _countdownAnimTime;
    public float CountdownSize => _countdownSize;

    [Header("GAMEPLAY")]
    [SerializeField] private bool _animateLivesLost = true;
    [SerializeField] private float _livesColorLerpTime;

    public bool AnimateLivesLost => _animateLivesLost;
    public float LivesColorLerpTime => _livesColorLerpTime;

    [Header("GAME OVER")]
    [SerializeField] private Sprite _endSpriteWin;
    [SerializeField] private Sprite _endSpriteLose;

    public Sprite EndSpriteWin => _endSpriteWin;
    public Sprite EndSpriteLose => _endSpriteLose;

}
