using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of game variables.
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("DEBUG")]
    [SerializeField] private bool _displayFPS;

    public bool DisplayFPS => _displayFPS;

    [Header("GENERAL")]
    [Tooltip("Time (s) to wait before revealing main menu. 0 might lead to laggy opening animation.")]
    [SerializeField][Range(0, 2)] private float _setupDelay;
    [Tooltip("Transition time (s) for UI panels to open or close.")]
    [SerializeField][Range(0, 2)] private float _panelFade;

    public float SetupDelay => _setupDelay;
    public float PanelFade => _panelFade;

    [Header("PRE-START")]
    [SerializeField] private bool _displayCountdown = true;
    [Tooltip("Text to be displayed in the countdown. Default: 3, 2, 1, GO!")]
    [SerializeField] private string[] _countdownStrings = {"3", "2", "1", "GO!"};
    [Tooltip("Time (s) it takes to cycle through each strings. Default = 1.")]
    [SerializeField][Range(0, 3)] private float _countdownDelay = 1;
    [Tooltip("Time (s) each string takes to scale up. Default = 0.5s")]
    [SerializeField][Range(0, 3)] private float _countdownAnimTime = 0.5f;
    [Tooltip("Target font size.")]
    [SerializeField] private float _countdownSize;

    public bool DisplayCountdown => _displayCountdown;
    public IReadOnlyList<string> CountdownStrings => _countdownStrings;
    public float CountdownDelay => _countdownDelay;
    public float CountdownAnimTime => _countdownAnimTime;
    public float CountdownSize => _countdownSize;

    [Header("BLOCK CONTROL")]
    [Tooltip("Number of units that fit horizontally in the game-area. Default: 18")]
    [SerializeField] private int _widthUnits;
    [Tooltip("Number of units that fit vertically in the game-area. Default: 40")]
    [SerializeField] private int _heightUnits;
    [Tooltip("Direction for 90 rotation. (-1 = Tetris rotation)")]
    [SerializeField][Range(-1, 1)] private int _rotateDirection;
    [Tooltip("Normal fall speed for the controller block.")]
    [SerializeField][Range(0.5f, 5)] private float _downSpeed;
    [Tooltip("Dragging down speed for the controller block.")]
    [SerializeField][Range(1, 20)] private float _dragDownSpeed;
    [Tooltip("Unit ratio that the controlled block snaps to when being dragged horizontally.")]
    [SerializeField][Range(0,1)] private float _dragSideSnap;
    [Tooltip("Max time (s) before release that a held down side movement is considered a swipe.")]
    [SerializeField][Range(0,1)] private float _swipeMaxTime;
    [Tooltip("Min units dragged to be considered a swipe.")]
    [SerializeField][Range(1, 10)] private float _swipeMinUnits;
    [Tooltip("Unit ratio that the controlled block snaps to when being swiped.")]
    [SerializeField][Range(1, 9)] private float _swipeSnap;

    public int WidthUnits => _widthUnits;
    public int HeightUnits => _heightUnits;
    public int RotateDir => _rotateDirection;
    public float DownSpeed => _downSpeed;
    public float DragDownSpeed => _dragDownSpeed;
    public float DragSideSnap => _dragSideSnap;
    public float SwipeMaxTime => _swipeMaxTime;
    public float SwipeMinUnits => _swipeMinUnits;
    public float SwipeSnap => _swipeSnap; 
}
