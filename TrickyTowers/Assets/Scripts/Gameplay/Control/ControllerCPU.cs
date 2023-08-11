using UnityEngine;

/// <summary>
/// Handles the CPU opponent's behaviour, in versus mode.
/// </summary>
public class ControllerCPU : ControllerAbstract
{
    // G A M E   O B J E C T

    private void OnEnable()
    {
        Block.OnPlaced += GetNewBlock;
        Block.OnOutOfBounds += HandleBlockLost;
    }

    private void OnDisable()
    {
        Block.OnPlaced -= GetNewBlock;
        Block.OnOutOfBounds -= HandleBlockLost;
    }

    private new void Update()
    {
        base.Update();

        if (!_isActive || _currentBlock == null) return;

        // Move block down at normal speed.
        _currentBlock.transform.Translate(
            Vector3.down * _gameData.NormalSpeed * Time.deltaTime, Space.World);
    }

    // M E T H O D S

    public new void Initialize()
    {
        base.Initialize();
    }

    public void Reset(Vector3 p_startPos)
    {
        FollowTower(false);
        transform.position = p_startPos;
    }

    public void GetNewBlock(int p_layer)
    {
        // Ignore if it's not a CPU block.
        if (p_layer != _blocksData.LayerCPU) return;

        base.HandleOldBlock();

        _currentBlock = _blockPool.GetCpuBlock();
        _currentBlock.Control(true);
    }

    private new void HandleBlockLost(Block p_block)
    {
        // Ignore if it's not a CPU block.
        if (p_block.gameObject.layer != _blocksData.LayerCPU || !_isActive)
            return;

        // If the lost block was the CPU's. Get a new one.
        if (p_block == _currentBlock) GetNewBlock(_blocksData.LayerCPU);

        base.HandleBlockLost(p_block);
    }
}
