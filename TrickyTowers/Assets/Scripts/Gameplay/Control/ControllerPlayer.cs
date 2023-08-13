using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the player input, during the main game loop.
/// </summary>
public class ControllerPlayer : ControllerAbstract
{
    // E V E N T S

    public static event Action<int, bool> OnLivesUpdated;

    // V A R I A B L E S

    [SerializeField] private GuideBeam _guideBeam;
    [SerializeField] private SavedDataSO _savedData;

    // Game session control.
    private int _currentLives;
    private float _coroutineTime;

    // Input control.
    private Vector3 _inputPressPos;
    private Vector3 _inputLastHoldPos;
    private float _inputPressTime;
    private float _inputWidth;
    private float _inputHeight;
    private float _inputDraggedX;
    private float _inputDraggedY;
    private int _inputDragDir;
    private bool _inputDraggingSide;
    private bool _inputDraggingDown;
    private bool _inputPressedThisBlock;
    private bool _isPaused;

    // G A M E   O B J E C T

    private void OnEnable()
    {
        Block.OnPlaced += GetNewBlock;
        Block.OnOutOfBounds += HandleBlockLost;
    }

    private void OnDisable()
    {
        Block.OnPlaced -= GetNewBlock;
        Block.OnOutOfBounds -= HandleBlockLost;
    }

    private new void Update()
    {
        base.Update();

        if (!_isActive || _isPaused || _currentBlock == null) return;

        // If being dragged down, move block down fast.
        if (_inputDraggingDown && _inputPressedThisBlock)
        {
            _currentBlock.transform.Translate(Vector3.down * 
                _gameData.DragDownSpeed * Time.deltaTime, Space.World);
        }

        // If not, move block down at normal speed.
        else _currentBlock.transform.Translate(
            Vector3.down * _gameData.NormalSpeed * Time.deltaTime, Space.World);

        // Input - On press.
        if (Input.GetMouseButtonDown(0))
        {
            _inputPressedThisBlock = true;
            _inputPressTime = Time.time;
            _inputPressPos = Input.mousePosition;
            _inputLastHoldPos = _inputPressPos;
        }

        // Input - On drag.
        else if (Input.GetMouseButton(0) && Input.mousePosition != _inputLastHoldPos)
        {
            if (!_inputPressedThisBlock) return;

            _inputDragDir = (Input.mousePosition.x - _inputLastHoldPos.x > 0) ? 1 : -1;
            _inputDraggedX = Mathf.Abs(Input.mousePosition.x - _inputLastHoldPos.x);
            _inputDraggedY = _inputLastHoldPos.y - Input.mousePosition.y;

            // If already dragging down.
            if (_inputDraggingDown) return;

            // User drags down.
            if (_inputDraggedY > _inputHeight)
            {
                _inputLastHoldPos = Input.mousePosition;
                _inputDraggingSide = false;
                _inputDraggingDown = true;
                _guideBeam.Lock();
            }

            // User drags sideways.
            else if (_inputDraggedX > (_inputWidth * _gameData.DragSideSnap))
            {
                _currentBlock.transform.Translate(
                    _gameData.DragSideSnap * _inputDragDir, 0, 0, Space.World);

                _inputLastHoldPos = Input.mousePosition;
                _inputDraggingSide = true;
                _inputDraggingDown = false;
            }
        }

        // Input - On released.
        else if (Input.GetMouseButtonUp(0))
        {
            if (!_inputPressedThisBlock) return;

            // If not being dragged.
            if (!(_inputDraggingSide || _inputDraggingDown))
            {
                // Rotate block.
                _currentBlock.transform.Rotate(0, 0, 90 * _gameData.RotateDir);
                _guideBeam.CounterRotation(-_gameData.RotateDir);
            }

            // If was being dragged to the side.
            else if (_inputDraggingSide)
            {
                // If it was swiped (fast movement).
                if (_inputDraggedX > _gameData.SwipeMinUnits &&
                    Time.time - _inputPressTime < _gameData.SwipeMaxTime)
                {
                    _currentBlock.transform.Translate(
                        _gameData.SwipeSnap * _inputDragDir, 0, 0, Space.World);
                }
            }

            _guideBeam.Unlock();
            _inputDraggingSide = false;
            _inputDraggingDown = false;
            _inputPressedThisBlock = false;
        }
    }

    // M E T H O D S

    public new void Initialize()
    {
        // Calculate input distances based on screen resolution.
        _inputWidth = Screen.width / _gameData.WidthUnits;
        _inputHeight = Screen.height / _gameData.HeightUnits;

        _guideBeam.Initialize();

        base.Initialize();
    }

    public void Reset(Vector3 p_startPos)
    {
        FollowTower(false);
        _isPaused = false;

        if (_gameData.InfiniteLives) _currentLives = -1;
        else _currentLives = _gameData.NumberOfLives;

        OnLivesUpdated?.Invoke(_currentLives, true);

        // Make camera pan circuit from finish line to starting base.
        StartCoroutine(PanDownFromFinish(p_startPos));
    }

    public new void Activate(bool p_active)
    {
        base.Activate(p_active);

        if (!p_active) _guideBeam.Stop();
    }

    public void GetNewBlock(int p_layer)
    {
        // Ignore if it's not a player block.
        if (p_layer != _blocksData.LayerPlayer) return;

        if (_currentBlock != null)
        {
            _inputDraggingSide = false;
            _inputDraggingDown = false;
            _inputPressedThisBlock = false;

            // Vibrate
            if (PlayerPrefs.GetInt(_savedData.VibrationPrefName) == 1)
                Vibration.VibratePop();
        }

        base.HandleOldBlock();

        _currentBlock = _blockPool.GetPlayerBlock();
        _currentBlock.Control(true);
        _guideBeam.Follow(_currentBlock);
    }

    private new void HandleBlockLost(Block p_block)
    {
        // Ignore if it's not a player block.
        if (p_block.gameObject.layer != _blocksData.LayerPlayer) 
            return;

        base.HandleBlockLost(p_block);

        if (!_isActive) return;

        _currentLives--;
        OnLivesUpdated?.Invoke(_currentLives, false);

        // If the lost block was the player's. Get a new one.
        if (p_block == _currentBlock)
        {
            _currentBlock = null;
            _inputDraggingSide = false;
            _inputDraggingDown = false;
            _inputPressedThisBlock = false;
            GetNewBlock(_blocksData.LayerPlayer);
        }
    }

    public void Pause(bool p_paused)
    {
        _inputPressedThisBlock = false;
        _isPaused = p_paused;
    }

    // C O R O U T I N E S

    private IEnumerator PanDownFromFinish(Vector3 p_startPos)
    {
        transform.position = p_startPos;
        _coroutineTime = 0;

        yield return new WaitForSeconds(_gameData.CamPreviewDelay);

        while (transform.position.y > 0)
        {
            transform.position = Vector3.Lerp(p_startPos, Vector3.zero, 
                _coroutineTime/_gameData.CamPreviewTime);

            _coroutineTime += Time.deltaTime;
            yield return null;
        }

        _coroutineTime = 0;
    }
}
