using UnityEngine;

/// <summary>
/// Manages the currently displayed UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private GameDataSO _data;

    [Header("PANELS")]
    [SerializeField] private UIPanelMainMenu _panelMainMenu;

    // G A M E   O B J E C T

    private void OnEnable() => Controller.OnNewGameState += UpdateDisplayUI;
    private void OnDisable() => Controller.OnNewGameState -= UpdateDisplayUI;

    // M E T H O D S

    private void UpdateDisplayUI(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.SETUP:

                Debug.Log("SETUP");
                CloseAllPanels();
                break;

            case GameState.MAIN_MENU:

                Debug.Log("MAIN MENU");
                _panelMainMenu.Open(_data.PanelFade);
                break;

            case GameState.PRE_START:

                Debug.Log("PRE-START");
                _panelMainMenu.Close(_data.PanelFade);
                break;

        }
    }

    private void CloseAllPanels()
    {
        _panelMainMenu.Close();
    }
}
