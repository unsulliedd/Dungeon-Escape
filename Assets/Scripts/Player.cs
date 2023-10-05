using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    private bool _isFacingRight;

    [Header("Jump Settings")]
    [SerializeField] private float _cayoteTime = 0.2f;
    [SerializeField] private float _cayoteTimeCounter;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _jumpBufferTimeCounter;
    [SerializeField] private int _remainingJumps = 1;
    [SerializeField] private GameObject _groundCheck;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravityScale = 2f;
    [SerializeField] private float _maxFallSpeed = 16f;
    [SerializeField] private float _fallMultiplier = 2f;

    [Header("References")]
    private Rigidbody2D _rigidBody2D;
    private PlayerAnimation _playerAnimation;
    private Vector2 _moveInput;

    void Awake()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
        if (!TryGetComponent(out _playerAnimation))
            Debug.Log("Player's PlayerAnimation is null");
    }

    void Update()
    {
        Flip();
        UpdateJumpCounters();
        if (_jumpBufferTimeCounter > 0 && (_remainingJumps > 0 || (_cayoteTimeCounter > 0 && IsGrounded())))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Gravity();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _playerAnimation.Run(_moveInput.x);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _jumpBufferTimeCounter = _jumpBufferTime;

        if (context.canceled)
            _jumpBufferTimeCounter = 0f;

        if (_jumpBufferTimeCounter > 0 && (_remainingJumps > 0 || (_cayoteTimeCounter > 0 && IsGrounded())))
        {
            Jump();
            _jumpBufferTimeCounter = 0f;
        }
    }

    private void Movement()
    {
        // Update the horizontal (x-axis) velocity of the Rigidbody2D.
        // The vertical (y-axis) velocity remains unchanged to preserve any existing vertical motion like gravity or jumping.
        _rigidBody2D.velocity = new Vector2(_moveInput.x * _speed, _rigidBody2D.velocity.y);
    }

    private void Jump()
    {
        // Reset the vertical velocity before applying the jump force to ensure consistent jump heights
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);

        // Apply the jump force
        _rigidBody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        // Decrement the remaining jumps
        _remainingJumps--;

        // Reset the jump buffer counter
        _jumpBufferTimeCounter = 0f;

        // Reset the cayote time counter
        if (_remainingJumps == 0)
            _cayoteTimeCounter = 0f;
    }

    private void Flip()
    {
        if (_moveInput.x < 0 && !_isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingRight = !_isFacingRight;
        }
        else if (_moveInput.x > 0 && _isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingRight = !_isFacingRight;
        }
    }

    private void Gravity()
    {
        if (_rigidBody2D.velocity.y < 0)
        {
            _rigidBody2D.gravityScale = _gravityScale * _fallMultiplier;            
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Max(_rigidBody2D.velocity.y, -_maxFallSpeed));
        }
        else
        {
               _rigidBody2D.gravityScale = _gravityScale;
        }
    }

    private void UpdateJumpCounters()
    {
        if (IsGrounded())
        {
            _cayoteTimeCounter = _cayoteTime;
            _remainingJumps = 1;
        }
        else
        {
            _cayoteTimeCounter -= Time.deltaTime;
        }
        _jumpBufferTimeCounter -= Time.deltaTime;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.transform.position, 0.27f, 1 << 8);
    }

    // Debugging
    private void OnDrawGizmos()
    {
        // Draw a circle at the ground check position to visualize the ground check radius
        Gizmos.DrawWireSphere(_groundCheck.transform.position, 0.27f);
    }
}