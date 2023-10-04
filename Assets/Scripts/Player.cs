using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float _cayoteTime = 0.2f;
    private float _cayoteTimeCounter;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;
    [SerializeField] private int _remainingJumps = 1;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravityScale = 2f;
    [SerializeField] private float _maxFallSpeed = 16f;
    [SerializeField] private float _fallMultiplier = 2f;


    [Header("References")]
    private Rigidbody2D _rigidBody2D;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _moveInput;

    void Awake()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
        if (!TryGetComponent(out _collider))
            Debug.Log("Player's BoxCollider2D is null");
        if (!GetComponentInChildren<SpriteRenderer>())
            Debug.Log("Player's SpriteRenderer is null");
        else
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
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
        Flip();
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
        if (_moveInput.x > 0)
            _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0)
            _spriteRenderer.flipX = true;
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
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, extraHeight, 1 << 8);
        return raycastHit.collider != null;
    }
}