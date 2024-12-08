using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D _rb;
    [Header("Movement")]
    public float _moveSpeed = 5f;
    float _horizontalMovement;
    [Header("Jumping")]
    public float _jumpPower = 8f;
    public int _maxJumps = 2;
    int _jumpRemaining;

    [Header("GroundCheck")]
    public Transform _groundCheckPos;
    public Vector2 _groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask _groundLayer;

    [Header("Gravity")]
    public float _baseGravity = 2f;
    public float _maxFallSpeed = 18f;
    public float _fallSpeedMultiplier = 2f;


    void Update()
    {
        _rb.velocity = new Vector2(_horizontalMovement * _moveSpeed, _rb.velocity.y);
        GroundCheck();
        Gravity();
    }
    private void Gravity()
    {
        if (_rb.velocity.y < 0)
        {
            // Fall increase faster
            _rb.gravityScale = _baseGravity * _fallSpeedMultiplier;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -_maxFallSpeed));
        }
        else
        {
            _rb.gravityScale = _baseGravity;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (_jumpRemaining > 0)
        {
            if (context.performed)
            {
                //Hold down jump button = full height jump
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
                _jumpRemaining--;
            }
            else if (context.canceled)
            {
                //Light tap of jump button = half height jump 
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
                _jumpRemaining--;
            }
        }

    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(_groundCheckPos.position, _groundCheckSize, 0, _groundLayer))
        {
            _jumpRemaining = _maxJumps;
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_groundCheckPos.position, _groundCheckSize);
    }
}
