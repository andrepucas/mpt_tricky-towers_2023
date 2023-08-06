using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Direct reference to each block. Toggles physics.
/// </summary>
public class FallingBlock : MonoBehaviour
{
    // E V E N T S

    public static event Action Placed;
    
    // V A R I A B L E S

    [SerializeField] private Rigidbody2D _rb;

    private bool _isControlled;

    // G A M E   O B J E C T

    private void OnCollisionEnter2D(Collision2D p_col)
    {
        if (_isControlled)
        {
            Control(false);
            Placed?.Invoke();
        }
    }
    
    // M E T H O D S

    public void Control(bool p_control)
    {
        _isControlled = p_control; 

        if (p_control) _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        else
        {
            _rb.velocity = Vector2.zero;
            _rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}
