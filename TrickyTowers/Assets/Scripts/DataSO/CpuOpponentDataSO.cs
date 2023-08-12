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
    [SerializeField] private CpuBlockRandomLimit[] _blockRandomLimits;

    private Dictionary<BlockType, List<int>> _blockRotLimits;
    private Dictionary<BlockType, List<float>> _blockPosLimits;

    public IReadOnlyList<float> RandomPositionsX => _randomPositionsX;
    public IReadOnlyDictionary<BlockType, List<int>> CpuLimitedRotOf => _blockRotLimits;
    public IReadOnlyDictionary<BlockType, List<float>> CpuLimitedPosOf => _blockPosLimits;

    public void InitializeDictionaries()
    {
        _blockRotLimits = new Dictionary<BlockType, List<int>>();
        _blockPosLimits = new Dictionary<BlockType, List<float>>();

        foreach(CpuBlockRandomLimit f_blockLimit in _blockRandomLimits)
        {
            _blockRotLimits.Add(f_blockLimit.Type, f_blockLimit.AllowedRotations);
            _blockPosLimits.Add(f_blockLimit.Type, f_blockLimit.AllowedPositionsX);
        }
    }
}

[System.Serializable]
public struct CpuBlockRandomLimit
{
    [SerializeField] private BlockType _type;
    [Tooltip("Pre-selected positions (x) for this block to land at. [0] will be used for first move.")]
    [SerializeField] private List<float> _allowedPositionsX;
    [Tooltip("Pre-selected rotations that this block is allowed to make by the CPU. [0] will be used for first move.")]
    [SerializeField] private List<int> _allowedRotations;

    public readonly BlockType Type => _type;
    public readonly List<float> AllowedPositionsX => _allowedPositionsX;
    public readonly List<int> AllowedRotations => _allowedRotations;
}
