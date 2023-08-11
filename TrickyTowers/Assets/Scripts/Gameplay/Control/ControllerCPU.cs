using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the CPU opponent's behaviour, in versus mode.
/// </summary>
public class ControllerCPU : ControllerAbstract
{
    // V A R I A B L E S

    [SerializeField] private CpuOpponentDataSO _cpuData;

    private int _randomIndex;
    private float _newTargetPosX;
    private float _moveDelay;
    private int _moveDirection;
    private int _rotationsNum;
    private float _rotateDelay;
    private YieldInstruction _actionStepDelay;

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

        // Move block down.
        _currentBlock.transform.Translate(Vector3.down * _gameData.NormalSpeed 
            * _cpuData.MoveSpeedMultiplier * Time.deltaTime, Space.World);
    }

    // M E T H O D S

    public new void Initialize()
    {
        _actionStepDelay = new WaitForSeconds(_cpuData.ActionStepTime);

        _cpuData.InitializeDictionaries();
        base.Initialize();
    }

    public void Reset(Vector3 p_startPos)
    {
        FollowTower(false);
        transform.position = p_startPos;
    }

    public new void Activate(bool p_active)
    {
        base.Activate(p_active);

        if (!p_active) StopAllCoroutines();
    }

    public void GetNewBlock(int p_layer)
    {
        // Ignore if it's not a CPU block.
        if (p_layer != _blocksData.LayerCPU) return;

        StopAllCoroutines();

        base.HandleOldBlock();

        _currentBlock = _blockPool.GetCpuBlock();
        _currentBlock.Control(true);

        if (_cpuData.Behaviour != CpuBehaviour.STACKER) RandomAction();
    }

    private void RandomAction()
    {
        // Define random position based on limits.
        _randomIndex = Random.Range(0, _cpuData.RandomPositionsX.Count);
        _newTargetPosX = _cpuData.RandomPositionsX[_randomIndex];

        if (_currentBlock.Type != BlockType.I && _currentBlock.Type != BlockType.O)
            _newTargetPosX += _gameData.DragSideSnap;
        
        if (_cpuData.Behaviour == CpuBehaviour.LIMIT_RANDOM)
        {
            // Define number of times rotated, based on the block limits defined.
            _rotationsNum = _cpuData.GetCpuBlockLimits[_currentBlock.Type]
                [Random.Range(0, _cpuData.GetCpuBlockLimits[_currentBlock.Type].Count)];
        }

        else _rotationsNum = Random.Range(0, 4);

        if (_newTargetPosX != 0) StartCoroutine(RandomMove());
        if (_rotationsNum != 0) StartCoroutine(RandomRotate());
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

    // C O R O U T I N E S

    private IEnumerator RandomMove()
    {
        _moveDelay = Random.Range(_cpuData.MinActionDelay, _cpuData.MaxActionDelay);
        yield return new WaitForSeconds(_moveDelay);

        _moveDirection = (_newTargetPosX > 0) ? 1 : -1;

        // Move to position with small grid steps, like the player.
        while (_currentBlock.transform.position.x != _newTargetPosX)
        {
            _currentBlock.transform.Translate(
                _gameData.DragSideSnap * _moveDirection, 0, 0, Space.World);

            yield return _actionStepDelay;
        }
    }

    private  IEnumerator RandomRotate()
    {
        _rotateDelay = Random.Range(_cpuData.MinActionDelay, _cpuData.MaxActionDelay);
        yield return new WaitForSeconds(_rotateDelay);

        // Rotate with a small delay between each rotation, so its visible.
        for (int i = 0; i < _rotationsNum; i++)
        {
            _currentBlock.transform.Rotate(0, 0, 90 * _gameData.RotateDir);
            yield return _actionStepDelay;
        }
    }
}
