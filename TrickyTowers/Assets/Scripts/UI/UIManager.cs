using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the currently displayed UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // E V E N T S

    public static event Action OnSetupComplete;

    // V A R I A B L E S

    [SerializeField] private UserInterfaceDataSO _uiData;
    [SerializeField] private GameObject _fpsCounter;

    [Header("PANELS")]
    [SerializeField] private UIPanelMainMenu _panelMainMenu;
    [SerializeField] private UIPanelPreStart _panelPreStart;
    [SerializeField] private UIPanelGameplay _panelGameplay;
    [SerializeField] private UIPanelPause _panelPause;
    [SerializeField] private UIPanelEnd _panelEnd;

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

                _fpsCounter.SetActive(_uiData.DisplayFPS);
                CloseAllPanels();
                StartCoroutine(SetupDelay());
                break;

            case GameState.MAIN_MENU:

                Debug.Log("MAIN MENU");

                if (_panelPause.IsOpen)
                {
                    _panelPause.Close();
                    _panelGameplay.Close();
                }

                else if (_panelEnd.IsOpen) _panelEnd.Close();

                _panelMainMenu.Open(_uiData.RevealFade);
                break;

            case GameState.PRE_START:

                Debug.Log("PRE-START");

                if (_panelPause.IsOpen)
                {
                    _panelPause.Close();
                    _panelGameplay.Close();
                }

                else if (_panelEnd.IsOpen) _panelEnd.Close();
                else _panelMainMenu.Close();

                _panelPreStart.Open(_uiData.PanelFade);
                break;

            case GameState.GAMEPLAY:

                Debug.Log("GAMEPLAY");

                if (_panelPause.IsOpen) _panelPause.Close();
                else _panelPreStart.Close(_uiData.PanelFade);

                _panelGameplay.Open(_uiData.PanelFade);
                break;

            case GameState.PAUSE:

                Debug.Log("PAUSE");

                _panelPause.Open(_uiData.PanelFade);
                break;

            case GameState.END_LOSE:

                Debug.Log("END LOSE");

                _panelGameplay.Close(_uiData.PanelFade);
                _panelEnd.OpenLose(_uiData.PanelFade);
                break;

        }
    }

    private void CloseAllPanels()
    {
        _panelMainMenu.Close();
        _panelPreStart.Close();
        _panelGameplay.Close();
        _panelPause.Close();
        _panelEnd.Close();
    }

    // C O R O U T I N E S

    private IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(_uiData.SetupDelay);
        OnSetupComplete?.Invoke();
    }
}
