using System;
using UnityEngine;

/// <summary>
/// Handles input and game state updates.
/// </summary>
public class Controller : MonoBehaviour
{
    // --- EVENTS ---

    public static event Action<GameState> OnNewGameState;

    // --- VARIABLES ---

    private bool _isVersusMode;

    // --- UNITY METHODS ---

    private void Awake() => UpdateGameState(GameState.SETUP);

    // --- CLASS METHODS ---

    private void UpdateGameState(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.SETUP:
                Debug.Log("SETUP");
                _isVersusMode = false;
                break;
        }

        OnNewGameState?.Invoke(p_state);
    }
}
