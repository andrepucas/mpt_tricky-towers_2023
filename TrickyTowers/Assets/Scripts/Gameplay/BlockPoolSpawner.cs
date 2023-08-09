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
    [SerializeField] private BlocksDataSO _blocksData;

    private const int POOL_HEIGHT = 100;

    private Dictionary<BlockType, GameObject> _blockPrefabs;
    private Dictionary<BlockType, List<Block>> _standbyPool;
    private List<Block> _activePool;

    private List<BlockType> _allTypes;
    private BlockType _lastBlockType;
    private BlockType _newBlockType;
    private Block _block;

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

    public Block GetBlock()
    {
        // If new block hasn't been pre-picked yet.
        if (_newBlockType == BlockType.NONE)
            _newBlockType = _allTypes[Random.Range(0, _allTypes.Count)];

        // If pool has this type of block, get it. 
        if (_standbyPool[_newBlockType].Count > 0)
            _block = _standbyPool[_newBlockType][0];

        // If not, create it.
        else _block = CreateBlock(_newBlockType);

        // Place it at the spawners position, in a static parent, and enable it.
        _block.transform.localPosition = Vector3.zero;
        _block.transform.rotation = Quaternion.identity;
        _block.transform.SetParent(_activeBlocksFolder);
        _block.gameObject.SetActive(true);

        // Manage pools.
        _standbyPool[_newBlockType].Remove(_block);
        _activePool.Add(_block);

        _lastBlockType = _newBlockType;
        PickNextBlock();

        return _block;
    }

    private void PickNextBlock()
    {
        // Pick random block, different from last.
        do {_newBlockType = _allTypes[Random.Range(0, _allTypes.Count)];}
        while (_newBlockType == _lastBlockType);

        OnNextBlockPicked?.Invoke(_blocksData.SpriteOf[_newBlockType], 
            _blocksData.ColorOf[_newBlockType]);
    }

    private Block CreateBlock(BlockType p_type)
    {
        return Instantiate(_blockPrefabs[p_type], Vector3.down * POOL_HEIGHT, 
            Quaternion.identity, transform).GetComponent<Block>();
    }
}
