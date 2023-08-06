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

    private bool _inVersusMode;

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

    // M E T H O D S

    private void UpdateGameState(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.SETUP:
                StartCoroutine(SetupDelay());
                break;

            case GameState.MAIN_MENU:
                _inVersusMode = false;
                break;

            case GameState.PRE_START:
                break;
        }

        OnNewGameState?.Invoke(p_state);
    }

    private void Play(bool p_versusMode)
    {
        _inVersusMode = p_versusMode;
        UpdateGameState(GameState.PRE_START);
    }

    // C O R O U T I N E S

    private IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(_data.SetupDelay);
        UpdateGameState(GameState.MAIN_MENU);
    }
}
