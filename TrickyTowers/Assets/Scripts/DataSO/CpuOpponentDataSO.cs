using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of NPC variables.
/// </summary>
[CreateAssetMenu(fileName = "CPU Opponent Data", menuName = "Data/CPU Data")]
public class CpuOpponentDataSO : ScriptableObject
{
    [Header("CPU BEHAVIOUR")]
    [SerializeField] private CpuBehaviour _behaviour;

    public CpuBehaviour Behaviour => _behaviour;

    [Header("GENERAL")]
    [Tooltip("How much faster the CPU blocks, compared to the player's normal block speed.")]
    [SerializeField][Range(1, 10)] private float _moveSpeedMultiplier;
    [Tooltip("Minimum delay (s) for the CPU to perform an action.")]
    [SerializeField][Range(0, 1)] private float _minActionDelay;
    [Tooltip("Maximum delay (s) for the CPU to perform an action.")]
    [SerializeField][Range(1, 10)] private float _maxActionDelay;
    [Tooltip("Time (s) in-between each step of the action. (Each rotation, or each move step.)")]
    [SerializeField][Range(0, 2)] private float _actionStepTime;

    public float MoveSpeedMultiplier => _moveSpeedMultiplier;
    public float MinActionDelay => _minActionDelay;
    public float MaxActionDelay => _maxActionDelay;
    public float ActionStepTime => _actionStepTime;

    [Header("RANDOM")]
    [Tooltip("X coordinates for the CPU to place blocks.")]
    [SerializeField] private float[] _randomPositionsX;
    [SerializeField] private CpuBlockRandomLimit[] _blockLimits;

    private Dictionary<BlockType, List<int>> _cpuBlockLimits;

    public IReadOnlyList<float> RandomPositionsX => _randomPositionsX;
    public IReadOnlyDictionary<BlockType, List<int>> GetCpuBlockLimits => _cpuBlockLimits;

    public void InitializeDictionaries()
    {
        _cpuBlockLimits = new Dictionary<BlockType, List<int>>();

        foreach(CpuBlockRandomLimit f_blockLimit in _blockLimits)
            _cpuBlockLimits.Add(f_blockLimit.Type, f_blockLimit.AllowedRotations);
    }
}

[System.Serializable]
public struct CpuBlockRandomLimit
{
    [SerializeField] private BlockType _type;
    [SerializeField] private List<int> _allowedRotations;

    public readonly BlockType Type => _type;
    public readonly List<int> AllowedRotations => _allowedRotations;
}
