using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of UI variables.
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("GENERAL")]
    [Tooltip("Time (s) to wait before revealing main menu. 0s might lead to laggy opening animation.")]
    [SerializeField][Range(0, 2)] private float _setupDelay;
    [Tooltip("Transition time (s) for UI panels to open or close.")]
    [SerializeField][Range(0, 2)] private float _panelFade;

    public float SetupDelay => _setupDelay;
    public float PanelFade => _panelFade;
}
