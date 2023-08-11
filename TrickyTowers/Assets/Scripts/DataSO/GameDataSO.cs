using UnityEngine;

/// <summary>
/// Data holder of game variables.
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("GENERAL")]
    [Tooltip("Number of blocks that can go out of bounds before losing.")]
    [SerializeField][Range(1, 9)] private int _numberOfLives = 5;
    [Tooltip("Enable this to play with infinite lives.")]
    [SerializeField] private bool _infiniteLives;
    [Tooltip("Height that tower must reach to win.")]
    [SerializeField][Range(0, 40)] private int _finishHeight = 40;

    public int NumberOfLives => _numberOfLives;
    public bool InfiniteLives => _infiniteLives;
    public int FinishHeight => _finishHeight;

    [Header("CAMERA")]
    [Tooltip("Time (s) for camera to pan from finish line to start base, at the start.")]
    [SerializeField][Range(0, 10)] private float _camPreviewTime;
    [Tooltip("Time (s) before camera starts panning from finish line to start base.")]
    [SerializeField][Range(0, 3)] private float _camPreviewDelay;
    [Tooltip("Time (s) for camera to try and update its position, based on tower height.")]
    [SerializeField][Range(.5f, 5)] private float _camFollowDelay;
    [Tooltip("Speed ratio at which the camera follows the tower. Closer to 0 => smoother follow.")]
    [SerializeField][Range(.2f, 2)] private float _camFollowSpeed;

    public float CamPreviewTime => _camPreviewTime;
    public float CamPreviewDelay => _camPreviewDelay;
    public float CamFollowDelay => _camFollowDelay;
    public float CamFollowSpeed => _camFollowSpeed;

    [Header("BLOCK CONTROL")]
    [Tooltip("Number of units that fit horizontally in the game-area. Default: 18")]
    [SerializeField] private int _widthUnits;
    [Tooltip("Number of units that fit vertically in the game-area. Default: 40")]
    [SerializeField] private int _heightUnits;
    [Tooltip("Direction for 90 rotation. (-1 = Tetris rotation)")]
    [SerializeField][Range(-1, 1)] private int _rotateDirection;
    [Tooltip("Normal fall speed for the controller block.")]
    [SerializeField][Range(0.5f, 5)] private float _normalSpeed;
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
    public float NormalSpeed => _normalSpeed;
    public float DragDownSpeed => _dragDownSpeed;
    public float DragSideSnap => _dragSideSnap;
    public float SwipeMaxTime => _swipeMaxTime;
    public float SwipeMinUnits => _swipeMinUnits;
    public float SwipeSnap => _swipeSnap;
}
