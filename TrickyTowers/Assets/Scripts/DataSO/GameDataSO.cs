using UnityEngine;

/// <summary>
/// Data holder of game variables.
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("GENERAL")]
    [Tooltip("Number of blocks that can go out of bounds before losing.")]
    [SerializeField][Range(1, 5)] private int _numberOfLives;
    [Tooltip("Enable this to play with infinite lives.")]
    [SerializeField] private bool _infiniteLives;

    public int NumberOfLives => _numberOfLives;
    public bool InfiniteLives => _infiniteLives;

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
