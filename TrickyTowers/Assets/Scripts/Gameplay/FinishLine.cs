using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects tower collision and starts or cancels the winning countdown.
/// </summary>
public class FinishLine : MonoBehaviour
{
    // E V E N T S

    public static event Action<bool> OnFinishLineAction;

    // V A R I A B L E S

    [SerializeField] private BlocksDataSO _blocksData;

    private HashSet<Transform> _blocksDetected;
    private bool _eventSent;

    // G A M E   O B J E C T

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.gameObject.tag == _blocksData.TagPlaced)
        {
            _blocksDetected.Add(p_other.transform.parent);

            if (_blocksDetected.Count > 0 && !_eventSent)
            {
                _eventSent = true;
                OnFinishLineAction?.Invoke(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D p_other)
    {
        if (p_other.gameObject.tag == _blocksData.TagPlaced)
        {
            _blocksDetected.Remove(p_other.transform.parent);

            if (_blocksDetected.Count <= 0 && _eventSent)
            {
                _eventSent = false;
                OnFinishLineAction?.Invoke(false);
            }
        }
    }

    // M E T H O D S

    public void Initialize(Vector3 p_position)
    {
        transform.position = p_position;
        _blocksDetected = new HashSet<Transform>();
    }

    public void Reset()
    {
        _blocksDetected.Clear();
        _eventSent = false;
    }
}