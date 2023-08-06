using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles input and game state updates.
/// </summary>
public class Controller : MonoBehaviour
{
    // E V E N T S

    public static event Action<GameState> OnNewGameState;

    // V A R I A B L E S

    [SerializeField] private GameDataSO _data;

    private GameState _currentState;
    [SerializeField] private FallingBlock _currentBlock;

    // Input.
    private Vector3 _inputLastPressPos;
    private float _inputWidth;
    private float _inputHeight;
    private float _inputDraggedX;
    private float _inputDraggedY;
    private int _inputDragDir;
    private bool _inputIsMoving;
    private bool _inputDraggingDown;

    // G A M E   O B J E C T

    private void Awake() => UpdateGameState(GameState.SETUP);

    private void OnEnable()
    {
        UIPanelMainMenu.OnPlayButtons += Play;
    }

    private void OnDisable()
    {
        UIPanelMainMenu.OnPlayButtons -= Play;
    }

    private void Update()
    {
        if (_currentState != GameState.GAMEPLAY) return;

        // Input - On press.
        if (Input.GetMouseButtonDown(0))
        {
            _inputLastPressPos = Input.mousePosition;
        }

        // Input - On drag.
        else if (Input.GetMouseButton(0) && Input.mousePosition != _inputLastPressPos)
        {
            _inputDragDir = (Input.mousePosition.x - _inputLastPressPos.x > 0) ? 1 : -1;
            _inputDraggedX = Mathf.Abs(Input.mousePosition.x - _inputLastPressPos.x);
            _inputDraggedY = _inputLastPressPos.y - Input.mousePosition.y;

            // Dragging sideways.
            if (_inputDraggedX > (_inputWidth * _data.BlockSideSnap))
            {
                _currentBlock.transform.Translate(
                    _data.BlockSideSnap * _inputDragDir, 0, 0, Space.World);

                _inputLastPressPos = Input.mousePosition;
                _inputIsMoving = true;
                _inputDraggingDown = false;
            }

            // Dragging down.
            else if (_inputDraggedY > _inputHeight)
            {
                _inputDraggingDown = true;
                _inputLastPressPos = Input.mousePosition;
                _inputIsMoving = true;
            }
        }

        // Input - On released.
        else if (Input.GetMouseButtonUp(0))
        {
            if (!_inputIsMoving)
            {
                // Rotate block.
                _currentBlock.transform.Rotate(0, 0, 90 * _data.RotateDir);
            }

            _inputIsMoving = false;
            _inputDraggingDown = false;
        }

        // If being dragged down, move block down fast.
        if (_inputDraggingDown) _currentBlock.transform.Translate(
            Vector3.down * _data.DragDownSpeed * Time.deltaTime, Space.World);

        // If not, move block down at normal speed.
        else _currentBlock.transform.Translate(
            Vector3.down * _data.FallSpeed * Time.deltaTime, Space.World);

    }

    // M E T H O D S

    private void UpdateGameState(GameState p_state)
    {
        _currentState = p_state;

        switch (_currentState)
        {
            case GameState.SETUP:

                _currentBlock.SetKinematic(true);
                _inputWidth = Screen.width / _data.WidthUnits;
                _inputHeight = Screen.height / _data.HeightUnits;

                StartCoroutine(SetupDelay());
                break;

            case GameState.MAIN_MENU:

                break;

            // case GameState.PRE_START:
            //     break;

            case GameState.GAMEPLAY:
                break;
        }

        OnNewGameState?.Invoke(_currentState);
    }

    private void Play(bool p_versusMode)
    {
        //_inVersusMode = p_versusMode;
        UpdateGameState(GameState.GAMEPLAY);
    }

    // C O R O U T I N E S

    private IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(_data.SetupDelay);
        UpdateGameState(GameState.MAIN_MENU);
    }
}
