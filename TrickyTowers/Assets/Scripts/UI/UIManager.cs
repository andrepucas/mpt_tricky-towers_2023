using UnityEngine;

/// <summary>
/// Manages the currently displayed UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private GameDataSO _data;
    [SerializeField] private GameObject _fpsCounter;

    [Header("PANELS")]
    [SerializeField] private UIPanelMainMenu _panelMainMenu;
    [SerializeField] private UIPanelPreStart _panelPreStart;
    [SerializeField] private UIPanelGameplay _panelGameplay;

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
                _fpsCounter.SetActive(_data.DisplayFPS);
                CloseAllPanels();
                break;

            case GameState.MAIN_MENU:

                Debug.Log("MAIN MENU");
                _panelMainMenu.Open(_data.PanelFade);
                break;

            case GameState.PRE_START:

                Debug.Log("PRE-START");
                _panelMainMenu.Close(_data.PanelFade);
                _panelPreStart.Open(_data.PanelFade);
                break;

            case GameState.GAMEPLAY:

                Debug.Log("GAMEPLAY");
                
                _panelPreStart.Close(_data.PanelFade);
                _panelGameplay.Open(_data.PanelFade);
                break;

        }
    }

    private void CloseAllPanels()
    {
        _panelMainMenu.Close();
        _panelPreStart.Close();
        _panelGameplay.Close();
    }
}
