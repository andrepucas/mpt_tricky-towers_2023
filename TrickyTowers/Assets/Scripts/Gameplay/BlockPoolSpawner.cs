using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockPoolSpawner : MonoBehaviour
{
    // E V E N T S

    public static event Action<Sprite, Color> OnNextBlockPicked;

    // V A R I A B L E S

    [SerializeField] private Transform _activeBlocksFolder;
    [SerializeField] private Transform _cpuSpawnTransform;
    [SerializeField] private BlocksDataSO _blocksData;

    private const int POOL_HEIGHT = 100;

    private Dictionary<BlockType, GameObject> _blockPrefabs;
    private Dictionary<BlockType, List<Block>> _standbyPool;
    private List<Block> _activePool;

    private List<BlockType> _allTypes;
    private BlockType _lastPlayerBlockType, _lastCpuBlockType;
    private BlockType _nextPlayerBlockType, _nextCpuBlockType;
    private Block _playerBlock, _cpuBlock;

    // M E T H O D S

    public void Initialize()
    {
        transform.localPosition = _blocksData.SpawnPos;

        _blockPrefabs = new Dictionary<BlockType, GameObject>();
        _standbyPool = new Dictionary<BlockType, List<Block>>();
        _activePool = new List<Block>();

        _allTypes = new List<BlockType>();

        foreach (BlockData f_block in _blocksData.AllBlocks)
        {
            _blockPrefabs.Add(f_block.Type, f_block.Prefab);
            _standbyPool.Add(f_block.Type, new List<Block>());

            _allTypes.Add(f_block.Type);

            for (int i = 0; i < _blocksData.InitialAmountEach; i++)
                SetStandby(CreateBlock(f_block.Type));
        }
    }

    public void SetStandby(Block p_block)
    {
        // Disable and add to standby pool of it's type.
        p_block.gameObject.SetActive(false);
        _standbyPool[p_block.Type].Add(p_block);

        // If object is active, remove it from active list.
        if (_activePool.Contains(p_block))
        {
            _activePool.Remove(p_block);
            p_block.transform.SetParent(transform);
        }
    }

    public void PoolAllActive()
    {
        List<Block> m_tempList = new(_activePool);
        
        foreach (Block f_block in m_tempList)
            SetStandby(f_block);
    }

    public Block GetPlayerBlock()
    {
        // If new block hasn't been pre-picked yet.
        if (_nextPlayerBlockType == BlockType.NONE)
            _nextPlayerBlockType = _allTypes[Random.Range(0, _allTypes.Count)];

        _playerBlock = GetBlock(_nextPlayerBlockType, _blocksData.LayerPlayer);
        _lastPlayerBlockType = _nextPlayerBlockType;
        PickNextPlayerBlock();

        return _playerBlock;
    }

    public Block GetCpuBlock()
    {
        // Pick a random block, different from the last.
        do {_nextCpuBlockType = _allTypes[Random.Range(0, _allTypes.Count)];}
        while (_nextCpuBlockType == _lastCpuBlockType);

        _cpuBlock = GetBlock(_nextCpuBlockType, _blocksData.LayerCPU);
        _lastCpuBlockType = _nextCpuBlockType;

        return _cpuBlock;
    }

    private Block GetBlock(BlockType p_blockType, int p_layer)
    {
        Block m_block;

        // If pool has this type of block, get it. 
        if (_standbyPool[p_blockType].Count > 0)
            m_block = _standbyPool[p_blockType][0];

        // If not, create it.
        else m_block = CreateBlock(_nextPlayerBlockType);

        // Prepare block.
        m_block.transform.SetParent(_activeBlocksFolder);
        m_block.transform.rotation = Quaternion.identity;

        // Spawn position depends on who's controlling it.
        if (p_layer == _blocksData.LayerPlayer)
            m_block.transform.position = transform.position;

        else m_block.transform.position = _cpuSpawnTransform.position;

        // Change layer game object & children colliders.
        m_block.gameObject.layer = p_layer;

        foreach(Transform f_child in m_block.transform)
            f_child.gameObject.layer = p_layer;

        // Enable it.
        m_block.gameObject.SetActive(true);

        // Manage pools.
        _standbyPool[p_blockType].Remove(m_block);
        _activePool.Add(m_block);

        return m_block;
    }

    private void PickNextPlayerBlock()
    {
        // Pick random block, different from last.
        do {_nextPlayerBlockType = _allTypes[Random.Range(0, _allTypes.Count)];}
        while (_nextPlayerBlockType == _lastPlayerBlockType);

        OnNextBlockPicked?.Invoke(_blocksData.SpriteOf[_nextPlayerBlockType], 
            _blocksData.ColorOf[_nextPlayerBlockType]);
    }

    private Block CreateBlock(BlockType p_type)
    {
        return Instantiate(_blockPrefabs[p_type], Vector3.down * POOL_HEIGHT, 
            Quaternion.identity, transform).GetComponent<Block>();
    }
}
