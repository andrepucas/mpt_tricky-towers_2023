using System.Collections.Generic;
using UnityEngine;

public class BlockPoolSpawner : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private BlocksDataSO _data;

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
        transform.localPosition = _data.SpawnPos;

        _blockPrefabs = new Dictionary<BlockType, GameObject>();
        _standbyPool = new Dictionary<BlockType, List<Block>>();
        _activePool = new List<Block>();

        _allTypes = new List<BlockType>();

        foreach (BlockData f_block in _data.AllBlocks)
        {
            _blockPrefabs.Add(f_block.Type, f_block.Prefab);
            _standbyPool.Add(f_block.Type, new List<Block>());

            _allTypes.Add(f_block.Type);

            for (int i = 0; i < _data.InitialAmountEach; i++)
                SetStandby(CreateBlock(f_block.Type));
        }
    }

    public void SetStandby(Block p_block)
    {
        // Disable and add to standby pool of it's type.
        p_block.gameObject.SetActive(false);
        _standbyPool[p_block.Type].Add(p_block);

        // If object is active, remove it from active list.
        if (_activePool.Contains(p_block)) _activePool.Remove(p_block);
    }

    public Block GetBlock()
    {
        // Pick random block, different from last.
        do {_newBlockType = _allTypes[Random.Range(0, _allTypes.Count)];}
        while (_newBlockType == _lastBlockType);

        _lastBlockType = _newBlockType;

        // If pool has this type of block, get it. 
        if (_standbyPool[_newBlockType].Count > 0)
            _block = _standbyPool[_newBlockType][0];

        // If not, create it.
        else _block = CreateBlock(_newBlockType);

        // Enable and place it at its spawn position.
        _block.transform.localPosition = Vector3.zero;
        _block.gameObject.SetActive(true);

        // Manage pools.
        _standbyPool[_newBlockType].Remove(_block);
        _activePool.Add(_block);

        return _block;
    }

    private Block CreateBlock(BlockType p_type)
    {
        return Instantiate(_blockPrefabs[p_type], Vector3.down * POOL_HEIGHT, 
            Quaternion.identity, transform).GetComponent<Block>();
    }
}