using UnityEngine;

/// <summary>
/// Data holder of UI variables.
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("DEBUG")]
    [SerializeField] private bool _displayFPS;

    public bool DisplayFPS => _displayFPS;

    [Header("GENERAL")]
    [Tooltip("Time (s) to wait before revealing main menu. 0s might lead to laggy opening animation.")]
    [SerializeField][Range(0, 2)] private float _setupDelay;
    [Tooltip("Transition time (s) for UI panels to open or close.")]
    [SerializeField][Range(0, 2)] private float _panelFade;

    public float SetupDelay => _setupDelay;
    public float PanelFade => _panelFade;

    [Header("INPUT")]
    [Tooltip("Number of units that fit horizontally in the game-area. Default: 18")]
    [SerializeField] private int _widthUnits;
    [Tooltip("Number of units that fit vertically in the game-area. Default: 40")]
    [SerializeField] private int _heightUnits;
    [Tooltip("Unit ratio that the controlled block snaps to, horizontally.")]
    [SerializeField][Range(0,1)] private float _blockSideSnap;
    [Tooltip("Dragging down speed for the controller block.")]
    [SerializeField][Range(1, 20)] private float _dragDownSpeed;
    [Tooltip("Direction for 90 rotation. (-1 = Tetris rotation)")]
    [SerializeField][Range(-1, 1)] private int _rotateDirection;

    public int WidthUnits => _widthUnits;
    public int HeightUnits => _heightUnits;
    public float BlockSideSnap => _blockSideSnap;
    public float DragDownSpeed => _dragDownSpeed;
    public int RotateDir => _rotateDirection;

    [Tooltip("Normal fall speed for the controller block.")]
    [SerializeField][Range(0.5f, 5)] private float _fallSpeed;

    public float FallSpeed => _fallSpeed;
}
