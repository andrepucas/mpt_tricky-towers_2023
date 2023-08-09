using System;
using UnityEngine;

/// <summary>
/// Handles input and game state updates.
/// </summary>
public class Controller : MonoBehaviour
{
    // E V E N T S

    public static event Action<GameState> OnNewGameState;
    public static event Action<int> OnLivesUpdated;

    // V A R I A B L E S

    [SerializeField] private BlockPoolSpawner _blockPool;
    [SerializeField] private GuideBeam _guideBeam;
    [SerializeField] private GameObject _gameplayObjs;

    [Header("DATA")]
    [SerializeField] private GameDataSO _data;
    [SerializeField] private BlocksDataSO _blocksData;

    private GameState _currentState;
    private Block _currentBlock;
    private int _currentLives;
    private bool _inVersusMode;

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

    // G A M E   O B J E C T

    private void Awake() => UpdateGameState(GameState.SETUP);

    private void OnEnable()
    {
        Block.OnPlaced += GetNewBlock;
        Block.OnOutOfBounds += HandleBlockLost;

        UIManager.OnSetupComplete += ToMenu;
        UIPanelMainMenu.OnPlayButtons += GameModePicked;
        UIPanelPreStart.OnCountdownEnd += Play;
        UIPanelGameplay.OnPauseButton += Pause;
        UIPanelPause.OnResumeButton += ToGameplay;
        UIPanelPause.OnRetryButton += StopAndRestart;
        UIPanelPause.OnQuitButton += StopAndQuit;
        UIPanelEnd.OnQuitButton += ToMenu;
        UIPanelEnd.OnRetryButton += ToPreStart;
    }

    private void OnDisable()
    {
        Block.OnPlaced -= GetNewBlock;
        Block.OnOutOfBounds -= HandleBlockLost;
        
        UIManager.OnSetupComplete -= ToMenu;
        UIPanelMainMenu.OnPlayButtons -= GameModePicked;
        UIPanelPreStart.OnCountdownEnd -= Play;
        UIPanelGameplay.OnPauseButton -= Pause;
        UIPanelPause.OnResumeButton -= ToGameplay;
        UIPanelPause.OnRetryButton -= StopAndRestart;
        UIPanelPause.OnQuitButton -= StopAndQuit;
        UIPanelEnd.OnQuitButton -= ToMenu;
        UIPanelEnd.OnRetryButton -= ToPreStart;
    }

    private void Update()
    {
        if (_currentState != GameState.GAMEPLAY || _currentBlock == null) return;

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

            // If dragging sideways.
            if (_inputDraggedX > (_inputWidth * _data.DragSideSnap))
            {
                _currentBlock.transform.Translate(
                    _data.DragSideSnap * _inputDragDir, 0, 0, Space.World);

                _inputLastHoldPos = Input.mousePosition;
                _inputDraggingSide = true;
                _inputDraggingDown = false;
            }

            // If dragging down.
            else if (_inputDraggedY > _inputHeight)
            {
                _inputLastHoldPos = Input.mousePosition;
                _inputDraggingSide = false;
                _inputDraggingDown = true;
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
                _currentBlock.transform.Rotate(0, 0, 90 * _data.RotateDir);
                _guideBeam.CounterRotation(-_data.RotateDir);
            }

            // If was being dragged to the side.
            else if (_inputDraggingSide)
            {
                // If it was swiped (fast movement).
                if (_inputDraggedX > _data.SwipeMinUnits &&
                    Time.time - _inputPressTime < _data.SwipeMaxTime)
                {
                    _currentBlock.transform.Translate(
                        _data.SwipeSnap * _inputDragDir, 0, 0, Space.World);
                }
            }

            _inputDraggingSide = false;
            _inputDraggingDown = false;
            _inputPressedThisBlock = false;
        }

        // If being dragged down, move block down fast.
        if (_inputDraggingDown && _inputPressedThisBlock)
        {
            _currentBlock.transform.Translate(Vector3.down * 
                _data.DragDownSpeed * Time.deltaTime, Space.World);
        }

        // If not, move block down at normal speed.
        else _currentBlock.transform.Translate(
            Vector3.down * _data.DownSpeed * Time.deltaTime, Space.World);
    }

    // M E T H O D S

    private void UpdateGameState(GameState p_state)
    {
        _currentState = p_state;

        switch (_currentState)
        {
            case GameState.SETUP:

                _inputWidth = Screen.width / _data.WidthUnits;
                _inputHeight = Screen.height / _data.HeightUnits;

                _blocksData.InitializeDictionaries();
                _blockPool.Initialize();
                _guideBeam.Initialize();
                break;

            case GameState.MAIN_MENU:

                Time.timeScale = 1;
                _gameplayObjs.SetActive(false);
                break;

            case GameState.PRE_START:

                Time.timeScale = 1;
                _blockPool.PoolAllActive();
                _gameplayObjs.SetActive(true);

                if (_data.InfiniteLives) _currentLives = -1;
                else _currentLives = _data.NumberOfLives;
                OnLivesUpdated?.Invoke(_currentLives);
                break;

            case GameState.GAMEPLAY:

                Time.timeScale = 1;
                break;

            case GameState.PAUSE:

                Time.timeScale = 0;
                break;

            case GameState.END_LOSE:

                StopGame();
                break;
        }

        OnNewGameState?.Invoke(_currentState);
    }

    private void StopGame()
    {
        _currentBlock.Control(false);
        _currentBlock = null;
        _guideBeam.Stop();
    }

    private void GetNewBlock()
    {
        _currentBlock = null;
        _inputPressedThisBlock = false;

        _currentBlock = _blockPool.GetBlock();
        _currentBlock.Control(true);
        _guideBeam.Follow(_currentBlock);
    }

    private void HandleBlockLost(Block p_block)
    {
        _currentLives--;
        OnLivesUpdated?.Invoke(_currentLives);

        // If no more lives. End game.
        if (_currentLives == 0) UpdateGameState(GameState.END_LOSE);

        // If the lost block was the player's. Get a new one.
        else if (p_block == _currentBlock) GetNewBlock();

        _blockPool.SetStandby(p_block);
    }

    private void GameModePicked(bool p_versusMode)
    {
        _inVersusMode = p_versusMode;
        UpdateGameState(GameState.PRE_START);
    }

    private void Play()
    {
        GetNewBlock();
        UpdateGameState(GameState.GAMEPLAY);
    }

    private void Pause()
    {
        _inputPressedThisBlock = false;
        UpdateGameState(GameState.PAUSE);
    }

    private void StopAndRestart()
    {
        StopGame();
        UpdateGameState(GameState.PRE_START);
    }

    private void StopAndQuit()
    {
        StopGame();
        UpdateGameState(GameState.MAIN_MENU);
    }

    private void ToMenu() => UpdateGameState(GameState.MAIN_MENU);
    private void ToPreStart() => UpdateGameState(GameState.PRE_START);
    private void ToGameplay() => UpdateGameState(GameState.GAMEPLAY);
}
