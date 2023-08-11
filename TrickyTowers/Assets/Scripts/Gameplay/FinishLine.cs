using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects tower collision and starts or cancels the winning countdown.
/// </summary>
public class FinishLine : MonoBehaviour
{
    // E V E N T S

    public static event Action<bool> OnWinAction;
    public static event Action<bool> OnLoseAction;

    // V A R I A B L E S

    [SerializeField] private BlocksDataSO _blocksData;

    private HashSet<Transform> _playerBlocksDetected;
    private HashSet<Transform> _cpuBlocksDetected;
    private bool _winEventSent;
    private bool _loseEventSent;

    // G A M E   O B J E C T

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.gameObject.tag == _blocksData.TagPlaced)
        {
            if (p_other.gameObject.layer == _blocksData.LayerPlayer)
            {
                _playerBlocksDetected.Add(p_other.transform.parent);

                if (_playerBlocksDetected.Count > 0 && !_winEventSent)
                {
                    _winEventSent = true;
                    OnWinAction?.Invoke(true);
                }
            }

            else if (p_other.gameObject.layer == _blocksData.LayerCPU)
            {
                _cpuBlocksDetected.Add(p_other.transform.parent);

                if (_cpuBlocksDetected.Count > 0 && !_loseEventSent)
                {
                    _loseEventSent = true;
                    OnLoseAction?.Invoke(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D p_other)
    {
        if (p_other.gameObject.tag == _blocksData.TagPlaced)
        {
            if (p_other.gameObject.layer == _blocksData.LayerPlayer)
            {
                _playerBlocksDetected.Remove(p_other.transform.parent);

                if (_playerBlocksDetected.Count <= 0 && _winEventSent)
                {
                    _winEventSent = false;
                    OnWinAction?.Invoke(false);
                }
            }

            else if (p_other.gameObject.layer == _blocksData.LayerCPU)
            {
                _cpuBlocksDetected.Remove(p_other.transform.parent);

                if (_cpuBlocksDetected.Count <= 0 && _loseEventSent)
                {
                    _loseEventSent = false;
                    OnLoseAction?.Invoke(false);
                }
            }
        }
    }

    // M E T H O D S

    public void Initialize(Vector3 p_position)
    {
        transform.position = p_position;
        _playerBlocksDetected = new HashSet<Transform>();
        _cpuBlocksDetected = new HashSet<Transform>();
    }

    public void Reset()
    {
        _playerBlocksDetected.Clear();
        _cpuBlocksDetected.Clear();
        _winEventSent = false;
    }
}