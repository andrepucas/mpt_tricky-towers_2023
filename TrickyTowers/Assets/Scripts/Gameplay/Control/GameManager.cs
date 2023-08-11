using System;
using UnityEngine;

/// <summary>
/// Handles game state updates and notifies game elements.
/// </summary>
public class GameManager : MonoBehaviour
{
    // E V E N T S

    public static event Action<GameState> OnNewGameState;
    public static event Action<bool> OnDisplayCPU;

    // V A R I A B L E S

    [Header("CONTROLLERS")]
    [SerializeField] private ControllerPlayer _playerController;
    [SerializeField] private ControllerCPU _cpuController;
    
    [Header("GAME ELEMENTS")]
    [SerializeField] private FinishLine _finishLine;
    [SerializeField] protected BlockPoolSpawner _blockPool;
    [SerializeField] private GameObject _toggleableGameObjs;

    [Header("DATA")]
    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private BlocksDataSO _blocksData;
    
    private GameState _currentState;
    private bool _inVersusMode;
    private Vector3 _finishPos;

    // G A M E   O B J E C T

    private void Awake() => UpdateGameState(GameState.SETUP);

    private void OnEnable()
    {
        ControllerPlayer.OnLivesUpdated += CheckGameOver;

        UIManager.OnSetupComplete += ToMenu;
        UIPanelMainMenu.OnPlayButtons += GameModePicked;
        UIPanelPreStart.OnCountdownEnd += Play;
        UIPanelGameplay.OnPauseButton += Pause;
        UIPanelGameplay.OnCountdownEnd += EndGame;
        UIPanelPause.OnResumeButton += ToGameplay;
        UIPanelPause.OnRetryButton += StopAndRestart;
        UIPanelPause.OnQuitButton += StopAndQuit;
        UIPanelEnd.OnQuitButton += StopAndQuit;
        UIPanelEnd.OnRetryButton += ToPreStart;
    }

    private void OnDisable()
    {
        ControllerPlayer.OnLivesUpdated -= CheckGameOver;

        UIManager.OnSetupComplete -= ToMenu;
        UIPanelMainMenu.OnPlayButtons -= GameModePicked;
        UIPanelPreStart.OnCountdownEnd -= Play;
        UIPanelGameplay.OnPauseButton -= Pause;
        UIPanelGameplay.OnCountdownEnd -= EndGame;
        UIPanelPause.OnResumeButton -= ToGameplay;
        UIPanelPause.OnRetryButton -= StopAndRestart;
        UIPanelPause.OnQuitButton -= StopAndQuit;
        UIPanelEnd.OnQuitButton -= StopAndQuit;
        UIPanelEnd.OnRetryButton -= ToPreStart;
    }

    // M E T H O D S

    private void UpdateGameState(GameState p_state)
    {
        _currentState = p_state;

        switch (_currentState)
        {
            case GameState.SETUP:

                // Initialize gameplay elements and data.
                _playerController.Initialize();
                _cpuController.Initialize();
                _blockPool.Initialize();

                _finishPos = Vector3.up * _gameData.FinishHeight;
                _playerController.transform.position = _finishPos;
                _finishLine.Initialize(_finishPos);

                _blocksData.InitializeDictionaries();

                _toggleableGameObjs.SetActive(false);

                break;

            case GameState.MAIN_MENU:

                Time.timeScale = 1;

                // Reset controller position.
                _playerController.transform.position = _finishPos;

                // Stop following blocks tower.
                _playerController.FollowTower(false);
                _cpuController.FollowTower(false);
                
                // Hide game elements.
                _toggleableGameObjs.SetActive(false);

                break;

            case GameState.PRE_START:

                Time.timeScale = 1;

                // Reset gameplay related data.
                _playerController.Reset(_finishPos);
                if (_inVersusMode) _cpuController.Reset(Vector3.zero);

                _blockPool.PoolAllActive();
                _finishLine.Reset();

                // Show game elements.
                _toggleableGameObjs.SetActive(true);

                break;

            case GameState.GAMEPLAY:

                Time.timeScale = 1;

                // Enable controllers.
                _playerController.Activate(true);

                if (_inVersusMode)
                {
                    _cpuController.Activate(true);
                    OnDisplayCPU?.Invoke(true);
                }

                else OnDisplayCPU?.Invoke(false);

                break;

            case GameState.PAUSE:

                Time.timeScale = 0;
                break;

            case GameState.END_LOSE:

                _playerController.Activate(false);
                if (_inVersusMode) _cpuController.Activate(false);
                break;

            case GameState.END_WIN:

                _playerController.Activate(false);
                if (_inVersusMode) _cpuController.Activate(false);
                break;
        }

        OnNewGameState?.Invoke(_currentState);
    }

    private void CheckGameOver(int p_lives)
    {
        if (p_lives == 0) UpdateGameState(GameState.END_LOSE);
    }

    private void GameModePicked(bool p_versusMode)
    {
        _inVersusMode = p_versusMode;
        UpdateGameState(GameState.PRE_START);
    }

    private void Play()
    {
        _playerController.GetNewBlock(_blocksData.LayerPlayer);
        if (_inVersusMode) _cpuController.GetNewBlock(_blocksData.LayerCPU);

        UpdateGameState(GameState.GAMEPLAY);
    }

    private void Pause()
    {
        _playerController.BreakInput();
        UpdateGameState(GameState.PAUSE);
    }

    private void StopAndRestart()
    {
        _playerController.Activate(false);
        if (_inVersusMode) _cpuController.Activate(false);

        UpdateGameState(GameState.PRE_START);
    }

    private void StopAndQuit()
    {
        _playerController.Activate(false);
        if (_inVersusMode) _cpuController.Activate(false);

        _toggleableGameObjs.SetActive(false);

        UpdateGameState(GameState.MAIN_MENU);
    }

    private void EndGame(bool p_result)
    {
        if (p_result) UpdateGameState(GameState.END_WIN);
        else UpdateGameState(GameState.END_LOSE);
    }

    private void ToPreStart() => UpdateGameState(GameState.PRE_START);
    private void ToMenu() => UpdateGameState(GameState.MAIN_MENU);
    private void ToGameplay() => UpdateGameState(GameState.GAMEPLAY);
}
