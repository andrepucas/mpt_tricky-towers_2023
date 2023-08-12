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

    [Header("PANELS")]
    [SerializeField] private UIPanelMainMenu _panelMainMenu;
    [SerializeField] private UIPanelSettings _panelSettings;
    [SerializeField] private UIPanelPreStart _panelPreStart;
    [SerializeField] private UIPanelGameplay _panelGameplay;
    [SerializeField] private UIPanelPause _panelPause;
    [SerializeField] private UIPanelEnd _panelEnd;

    [Header("SHOW FPS")]
    [SerializeField] private GameObject _fpsCounter;

    [Header("DATA")]
    [SerializeField] private UserInterfaceDataSO _uiData;
    [SerializeField] private SavedDataSO _savedData;

    // G A M E   O B J E C T

    private void OnEnable() => GameManager.OnNewGameState += UpdateDisplayUI;
    private void OnDisable() => GameManager.OnNewGameState -= UpdateDisplayUI;

    // M E T H O D S

    private void UpdateDisplayUI(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.SETUP:

                SetupPanels();
                StartCoroutine(SetupDelay());
                break;

            case GameState.MAIN_MENU:

                // Show FPS.
                if (PlayerPrefs.GetInt(_savedData.ShowFpsPrefName, _savedData.ShowFPSDefault) == 0)
                    _fpsCounter.SetActive(false);

                else _fpsCounter.SetActive(true);

                // Close opened panels.
                if (_panelPause.IsOpen)
                {
                    _panelPause.Close(_uiData.PanelFade);
                    _panelGameplay.Close(_uiData.PanelFade);
                }

                else if (_panelEnd.IsOpen) _panelEnd.Close(_uiData.PanelFade);

                else if (_panelSettings.IsOpen) _panelSettings.Close(_uiData.PanelFade);

                // Open menu panel.
                _panelMainMenu.Open(_uiData.RevealFade);
                break;

            case GameState.SETTINGS:

                // Always disable FPS counter inside settings.
                _fpsCounter.SetActive(false);

                _panelMainMenu.Close(_uiData.PanelFade);
                _panelSettings.Open(_uiData.PanelFade);
                break;

            case GameState.PRE_START:

                // Close opened panels.
                if (_panelPause.IsOpen)
                {
                    _panelPause.Close(_uiData.PanelFade);
                    _panelGameplay.Close(_uiData.PanelFade);
                }

                else if (_panelEnd.IsOpen) _panelEnd.Close(_uiData.PanelFade);
                else _panelMainMenu.Close(_uiData.PanelFade);

                // Open pre-start panel.
                _panelPreStart.Open(_uiData.PanelFade);
                break;

            case GameState.GAMEPLAY:

                if (_panelPause.IsOpen) _panelPause.Close(_uiData.PanelFade);
                else _panelPreStart.Close(_uiData.PanelFade);

                _panelGameplay.Open(_uiData.PanelFade);
                break;

            case GameState.PAUSE:

                _panelPause.Open(_uiData.PanelFade);
                break;

            case GameState.END_LOSE:

                _panelGameplay.Close(_uiData.PanelFade);
                _panelEnd.OpenLose(_uiData.PanelFade);
                break;

            case GameState.END_WIN:

                _panelGameplay.Close(_uiData.PanelFade);
                _panelEnd.OpenWin(_uiData.PanelFade);
                break;

        }
    }

    private void SetupPanels()
    {
        _panelMainMenu.Setup();
        _panelSettings.Setup();
        _panelPreStart.Setup();
        _panelGameplay.Setup();
        _panelPause.Setup();
        _panelEnd.Setup();
    }

    // C O R O U T I N E S

    private IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(_uiData.SetupDelay);
        OnSetupComplete?.Invoke();
    }
}
