using UnityEngine;

/// <summary>
/// Direct reference to each block. Toggles physics.
/// </summary>
public class FallingBlock : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private Rigidbody2D _rb;

    // M E T H O D S

    public void SetKinematic(bool p_active) => _rb.isKinematic = p_active;
}
