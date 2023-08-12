using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles base behaviours shared by both Player & CPU controllers.
/// </summary>
public abstract class ControllerAbstract : MonoBehaviour
{
    // V A R I A B L E S

    [Header("DATA")]
    [SerializeField] protected GameDataSO _gameData;
    [SerializeField] protected BlocksDataSO _blocksData;

    [Header("GAME ELEMENTS")]
    [SerializeField] protected BlockPoolSpawner _blockPool;

    protected List<Transform> _towerTransforms;
    protected Block _currentBlock;
    protected bool _isFollowingTower;
    protected bool _isActive;
    protected float _targetHeight;
    private float _elapsedTime;

    // G A M E   O B J E C T

    protected void Update()
    {
        // Camera tracking tallest block.
        if (_isFollowingTower)
        {
            if (_elapsedTime > _gameData.CamFollowDelay)
            {
                _elapsedTime = 0;
                _targetHeight = 0;

                for (int i = 0; i < _towerTransforms.Count; i++)
                {
                    if (_towerTransforms[i].position.y > _targetHeight)
                        _targetHeight = _towerTransforms[i].position.y;
                }
            }

            if (_targetHeight >= 0 && (_targetHeight != transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    Vector3.up * _targetHeight, 
                    Time.deltaTime * _gameData.CamFollowSpeed);
            }

            _elapsedTime += Time.deltaTime;
        }
    }

    // M E T H O D S

    public void Activate(bool p_active)
    {
        _isActive = p_active;

        if (_isActive) FollowTower(true);

        else if (_currentBlock != null)
        {
            _currentBlock.Control(false);
            _currentBlock = null;
        }
    }

    protected void Initialize()
    {
        _towerTransforms = new List<Transform>();
    }

    public void FollowTower(bool p_follow)
    {
        _isFollowingTower = p_follow;

        if (!_isFollowingTower)
        {
            _targetHeight = 0;
            _towerTransforms.Clear();
        }
    }

    protected void HandleOldBlock()
    {
        if (_currentBlock != null) 
            _towerTransforms.Add(_currentBlock.transform);

        else _currentBlock = null;
    }

    protected void HandleBlockLost(Block p_block)
    {
        _blockPool.SetStandby(p_block);
        _towerTransforms.Remove(p_block.transform);
    }
}
