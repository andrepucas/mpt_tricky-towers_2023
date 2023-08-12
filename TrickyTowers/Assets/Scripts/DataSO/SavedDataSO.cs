using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of saved settings variables. Settings themselves are stored in PlayerPrefs.
/// </summary>
[CreateAssetMenu(fileName = "Saved Data", menuName = "Data/Saved Data")]
public class SavedDataSO : ScriptableObject
{
    [Header("FPS TARGET")]
    [Tooltip("Name to save and load FPS value in Player Prefs.")]
    [SerializeField] private string _fpsPrefName;
    [Tooltip("FPS target options available to pick.")]
    [SerializeField] private int[] _fpsOptions;
    [Tooltip("FPS target default value. (Index of options list)")]
    [SerializeField][Range(0, 2)] private int _fpsDefault;

    public string FpsPrefName => _fpsPrefName;
    public IReadOnlyList<int> FpsOptions => _fpsOptions;
    public int FpsDefault => _fpsDefault;

    [Header("SHOW FPS")]
    [Tooltip("Name to save and load Show FPS toggle in Player Prefs.")]
    [SerializeField] private string _showFpsPrefName;
    [Tooltip("Show FPS default toggle. 0 = OFF, 1 = ON")]
    [SerializeField][Range(0, 1)] private int _showFpsDefault;

    public string ShowFpsPrefName => _showFpsPrefName;
    public int ShowFPSDefault => _showFpsDefault;
}
