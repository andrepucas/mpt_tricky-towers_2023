using UnityEngine;

/// <summary>
/// Vibrates and plays sounds for UI elements.
/// </summary>
public class FeedbackManager : MonoBehaviour
{
    // V A R I A B L E S

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource _uiBtnClick;
    [SerializeField] private AudioSource _uiCountdownPop;
    [SerializeField] private AudioSource _uiCountdownEnd;
    [SerializeField] private AudioSource _blockPlaced;
    [SerializeField] private AudioSource _blockLost;
    
    [Header("DATA")]
    [SerializeField] private SavedDataSO _savedData;

    // M E T H O D S

    public void OnUIButtonClick()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _uiBtnClick.Play();

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePop();
    }

    public void OnUICountdownPop()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _uiCountdownPop.Play();

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePop();
    }

    public void OnUICountdownPeek()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _uiCountdownEnd.Play();
        
        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePeek();
    }

    public void OnBlockPlaced()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
        {
            _blockPlaced.pitch = Random.Range(2f, 3f);
            _blockPlaced.Play();
        }

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePeek();
    }

    public void OnBlockLost()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _blockLost.Play();

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibrateNope();
    }
}

