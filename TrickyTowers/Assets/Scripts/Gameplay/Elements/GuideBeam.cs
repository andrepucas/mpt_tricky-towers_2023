using UnityEngine;

/// <summary>
/// Handles a single guide beam, that adjusts to the controlled block.
/// </summary>
public class GuideBeam : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BlocksDataSO _blocksData;

    private Transform _parent;
    private BlockType _currentType;
    private Vector2 _auxSize;
    private Vector2 _auxPos;
    private int _rotationIndex;

    // M E T H O D S

    public void Initialize()
    {
        _renderer.enabled = false;
        _parent = transform.parent;

        _auxSize = new Vector2(0, 200);
        _auxPos = Vector2.zero;
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
        // _renderer.color = _blocksData.ColorOf[_currentType];

        // Width.
        _auxSize.x = _blocksData.WidthOf[_currentType][_rotationIndex];
        _renderer.size = _auxSize;

        // Offset.
        _auxPos.y = _blocksData.OffsetOf[_currentType][_rotationIndex];
        transform.localPosition = _auxPos;
    }

    public void Stop()
    {
        _renderer.enabled = false;
        transform.SetParent(_parent);
    }
}