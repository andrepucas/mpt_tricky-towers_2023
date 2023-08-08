using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles a single guide beam, that adjusts to the controlled block.
/// </summary>
public class GuideBeam : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BlocksDataSO _blocksData;

    private Dictionary<BlockType, Color> _blockColors;
    private Dictionary<BlockType, List<int>> _blockWidths;
    private Dictionary<BlockType, List<float>> _blockOffsets;

    private BlockType _currentType;
    private Vector2 _auxSize;
    private Vector2 _auxPos;
    private int _rotationIndex;

    // M E T H O D S

    public void Initialize()
    {
        _renderer.enabled = false;

        _auxSize = new Vector2(0, 200);
        _auxPos = Vector2.zero;

        _blockColors = new Dictionary<BlockType, Color>();
        _blockWidths = new Dictionary<BlockType, List<int>>();
        _blockOffsets = new Dictionary<BlockType, List<float>>();

        foreach (BlockData _blockData in _blocksData.AllBlocks)
        {
            _blockColors.Add(_blockData.Type, _blockData.Color);
            _blockWidths.Add(_blockData.Type, _blockData.WidthLoop);
            _blockOffsets.Add(_blockData.Type, _blockData.OffsetLoop);
        }
    }

    public void Follow(Block p_block)
    {
        _currentType = p_block.Type;
        _rotationIndex = 0;

        // Attach to block.
        transform.SetParent(p_block.transform);

        AdaptToBlock();
        _renderer.enabled = true;
    }

    public void CounterRotation(int p_direction)
    {
        transform.Rotate(0, 0, -90 * p_direction, Space.Self);

        _rotationIndex += p_direction;
        if (_rotationIndex == -1) _rotationIndex = 3;
        else if (_rotationIndex == 4) _rotationIndex = 0;

        AdaptToBlock();
    }

    private void AdaptToBlock()
    {
        // Color.
        _renderer.color = _blockColors[_currentType];

        // Width.
        _auxSize.x = _blockWidths[_currentType][_rotationIndex];
        _renderer.size = _auxSize;

        // Offset.
        _auxPos.y = _blockOffsets[_currentType][_rotationIndex];
        transform.localPosition = _auxPos;
    }
}