using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder of block variables.
/// </summary>
[CreateAssetMenu(fileName = "Blocks Data", menuName = "Data/Blocks Data")]
public class BlocksDataSO : ScriptableObject
{
    [Header("BLOCKS")]
    [SerializeField] private BlockData[] _allBlocks;
    [Tooltip("Position, relative to the camera, for the controlled block to appear at.")]
    [SerializeField] private Vector2 _spawnPos;
    [Tooltip("Amount to have pre-initialized in the standby pool, of each block type.")]
    [SerializeField][Range(1, 20)] private int _initialAmountEach;

    public IReadOnlyList<BlockData> AllBlocks => _allBlocks;
    public Vector2 SpawnPos => _spawnPos;
    public int InitialAmountEach => _initialAmountEach;
}

[System.Serializable]
public struct BlockData
{
    [SerializeField] private BlockType _type;
    [SerializeField] private GameObject _prefab;

    public readonly BlockType Type => _type;
    public readonly GameObject Prefab => _prefab;
}
