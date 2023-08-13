using UnityEngine;

/// <summary>
/// Vibrates and plays sounds for UI elements.
/// </summary>
public class FXFeedback : MonoBehaviour
{
    // V A R I A B L E S

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource _uiBtnClick;
    [SerializeField] private AudioSource _uiCountdownPop;
    [SerializeField] private AudioSource _uiCountdownEnd;
    [SerializeField] private AudioSource _blockPlaced;
    [SerializeField] private AudioSource _blockLost;
    [SerializeField] private AudioSource _blockRotate;
    [SerializeField] private AudioSource _blockLocked;
    [SerializeField] private AudioSource _win;
    [SerializeField] private AudioSource _lose;

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

    public void OnBlockPlaced(bool p_fast)
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
        {
            if (p_fast) _blockPlaced.pitch = Random.Range(.4f, .6f);
            else _blockPlaced.pitch = Random.Range(2f, 3f);

            _blockPlaced.Play();
        }

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
        {
            if (p_fast) Vibration.VibratePeek();
            else Vibration.VibratePop();
        }
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

    public void OnBlockRotate()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
        {
            _blockRotate.pitch = Random.Range(.8f, 1.5f);
            _blockRotate.Play();
        }

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePop();
    }

    public void OnBlockLocked()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
        {
            _blockLocked.pitch = Random.Range(2f, 3f);
            _blockLocked.Play();
        }

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.VibratePop();
    }

    public void OnWin()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _win.Play();

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.Vibrate();
    }

    public void OnLose()
    {
        // Play sound.
        if (PlayerPrefs.GetInt(_savedData.SfxPrefName, _savedData.SfxDefault) == 1)
            _lose.Play();

        // Vibrate.
        if (PlayerPrefs.GetInt(_savedData.VibrationPrefName, _savedData.VibrationDefault) == 1)
            Vibration.Vibrate();
    }
}

