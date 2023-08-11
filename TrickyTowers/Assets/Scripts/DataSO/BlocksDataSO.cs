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
    [Tooltip("Collider radius of the controlled block. A value smaller than 0.1 (static collider size) allows for slight interception with static blocks.")]
    [SerializeField][Range(0.01f, 0.1f)] private float _controlledBlockRadius = 0.05f;
    [Tooltip("Name of the losing area colliders.")]
    [SerializeField] private string _tagLimits;
    [Tooltip("Name of the tag to apply to colliders when placed.")]
    [SerializeField] private string _tagPlaced;
    [Tooltip("Layer (int) of the blocks used by the player.")]
    [SerializeField] private int _layerPlayer;
    [Tooltip("Layer (int) of the blocks used by the CPU.")]
    [SerializeField] private int _layerCpu;

    public IReadOnlyList<BlockData> AllBlocks => _allBlocks;
    public float ControlledBlockRadius => _controlledBlockRadius;
    public string TagLimits => _tagLimits;
    public string TagPlaced => _tagPlaced;
    public int LayerPlayer => _layerPlayer;
    public int LayerCPU => _layerCpu;

    [Header("POOLING")]
    [Tooltip("Position, relative to the camera, for the controlled block to appear at.")]
    [SerializeField] private Vector2 _spawnPos;
    [Tooltip("Amount to have pre-initialized in the standby pool, of each block type.")]
    [SerializeField][Range(1, 20)] private int _initialAmountEach;

    public Vector2 SpawnPos => _spawnPos;
    public int InitialAmountEach => _initialAmountEach;

    // D I C T I O N A R I E S

    private Dictionary<BlockType, Color> _blockColors;
    private Dictionary<BlockType, Sprite> _blockSprites;
    private Dictionary<BlockType, List<int>> _blockWidths;
    private Dictionary<BlockType, List<float>> _blockOffsets;

    public IReadOnlyDictionary<BlockType, Color> ColorOf => _blockColors;
    public IReadOnlyDictionary<BlockType, Sprite> SpriteOf => _blockSprites;
    public IReadOnlyDictionary<BlockType, List<int>> WidthOf => _blockWidths;
    public IReadOnlyDictionary<BlockType, List<float>> OffsetOf => _blockOffsets;

    public void InitializeDictionaries()
    {
        _blockColors = new Dictionary<BlockType, Color>();
        _blockSprites = new Dictionary<BlockType, Sprite>();
        _blockWidths = new Dictionary<BlockType, List<int>>();
        _blockOffsets = new Dictionary<BlockType, List<float>>();

        foreach (BlockData _blockData in AllBlocks)
        {
            _blockColors.Add(_blockData.Type, _blockData.Color);
            _blockSprites.Add(_blockData.Type, _blockData.Sprite);
            _blockWidths.Add(_blockData.Type, _blockData.WidthLoop);
            _blockOffsets.Add(_blockData.Type, _blockData.OffsetLoop);
        }
    }
}

[System.Serializable]
public struct BlockData
{
    [SerializeField] private BlockType _type;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Color _color;
    [SerializeField] private Sprite _sprite;
    [Tooltip("Block width every time it rotates.")]
    [SerializeField][Range(1, 4)] private List<int> _widthLoop;
    [Tooltip("Block offset every time it rotates.")]
    [SerializeField][Range(-0.5f, 0.5f)] private List<float> _offsetLoop;

    public readonly BlockType Type => _type;
    public readonly GameObject Prefab => _prefab;
    public readonly Color Color => _color;
    public readonly Sprite Sprite => _sprite;
    public readonly List<int> WidthLoop => _widthLoop;
    public readonly List<float> OffsetLoop => _offsetLoop;
}
