using UnityEngine;

/// <summary>
/// Manages the currently displayed UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // --- UNITY METHODS ---

    private void OnEnable() => Controller.OnNewGameState += UpdateDisplayUI;
    private void OnDisable() => Controller.OnNewGameState -= UpdateDisplayUI;

    // --- CLASS METHODS ---

    private void UpdateDisplayUI(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.SETUP:
                
                Debug.Log("UI SETUP");
                break;
        }
    }
}
