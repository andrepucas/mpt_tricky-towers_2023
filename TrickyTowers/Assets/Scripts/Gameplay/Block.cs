using System;
using UnityEngine;

/// <summary>
/// Direct reference to each block. Toggles physics.
/// </summary>
public class Block : MonoBehaviour
{
    // E V E N T S

    public static event Action<int> OnPlaced;
    public static event Action<Block> OnOutOfBounds;

    // V A R I A B L E S

    private const float STATIC_RADIUS = 0.1f;

    [SerializeField] private BlockType _type;

    [Header("COLLISIONS")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D[] _colliders;

    [Header("DATA")]
    [SerializeField] private BlocksDataSO _blocksData;

    public BlockType Type => _type;
    private bool _isControlled;
    private bool _isOutOfBounds;

    // G A M E   O B J E C T

    private void OnCollisionEnter2D(Collision2D p_other)
    {
        if (!_isOutOfBounds)
        {
            if (p_other.gameObject.tag == _blocksData.TagLimits)
            {
                _isOutOfBounds = true;
                Control(false);
                OnOutOfBounds?.Invoke(this);
            }

            else if (_isControlled)
            {
                Control(false);
                OnPlaced?.Invoke(gameObject.layer);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.gameObject.tag == _blocksData.TagLoseAnchor && !_isOutOfBounds
            && _isControlled)
        {
            _isOutOfBounds = true;
            Control(false);
            OnOutOfBounds?.Invoke(this);
        }
    }

    // M E T H O D S

    public void Control(bool p_control)
    {
        _isControlled = p_control; 

        if (p_control)
        {
            foreach(BoxCollider2D f_collider in _colliders)
            {
                f_collider.edgeRadius = _blocksData.ControlledBlockRadius;
                f_collider.tag = "Untagged";
            }

            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _isOutOfBounds = false;
        }

        else
        {
            foreach(BoxCollider2D f_collider in _colliders)
            {
                f_collider.edgeRadius = STATIC_RADIUS;
                f_collider.tag = _blocksData.TagPlaced;

                // Refresh trigger detections after tag change.
                f_collider.enabled = false;
                f_collider.enabled = true;
            }

            _rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}
