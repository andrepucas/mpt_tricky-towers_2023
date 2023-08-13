using UnityEngine;
using TMPro;

/// <summary>
/// FPS counter.
/// </summary>
public class FPSCounter : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private float timeToUpdateCounter = 1f;
    [SerializeField] private TextMeshProUGUI _fpsText; 

    private float _elapsedTime;
    private int _frameCounter;
    private int _frameRate;

    // G A M E   O B J E C T

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _frameCounter++;

        // If current time as reached time to calculate fps.
        if (_elapsedTime >= timeToUpdateCounter)
        {
            // Checks how many frames have been counted.
            _frameRate = Mathf.RoundToInt(_frameCounter / _elapsedTime);

            _fpsText.text = _frameRate.ToString();

            _elapsedTime -= timeToUpdateCounter;
            _frameCounter = 0;
        }
    }
}