using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpForce = 16f;    
    private float _horizontalMoveInput;
    private bool _isFacingRight;

    [Header("Jump Settings")]
    [SerializeField] private float _cayoteTime = 0.1f;
    [SerializeField] private float _cayoteTimeCounter;
    [SerializeField] private float _jumpBufferTime = 0.1f;
    [SerializeField] private float _jumpBufferTimeCounter;
    [SerializeField] private bool _canSecondJump;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckRadius = 0.27f;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundLayerMask;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravityScale = 2f;
    [SerializeField] private float _maxFallSpeed = 16f;
    [SerializeField] private float _fallMultiplier = 2f;

    [Header("Attack Settings")]
    [SerializeField] private bool _groundAttack;

    [Header("References")]
    private Rigidbody2D _rigidBody2D;
    private PlayerAnimation _playerAnimation;

    void Awake()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
        if (!TryGetComponent(out _playerAnimation))
            Debug.Log("Player's PlayerAnimation is null");
    }

    void Update()
    {
        _isGrounded = IsGrounded();

        Flip();
        UpdateJumpCounters();
    }

    private void FixedUpdate()
    {
        Movement();
        Gravity();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _horizontalMoveInput = context.ReadValue<Vector2>().x;
        _playerAnimation.RunAnimation(_horizontalMoveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _jumpBufferTimeCounter = _jumpBufferTime;
        else if (context.performed)
        {
            if (_cayoteTimeCounter > 0f && _jumpBufferTimeCounter > 0f && !_groundAttack)
            {                
                _canSecondJump = true;
                HandleJump();
            }
            else if (_canSecondJump && _rigidBody2D.velocity.y > 0f && !_groundAttack)
            {
                _canSecondJump = false;
                HandleJump();
            }
        }
        else if (context.canceled)
            _cayoteTimeCounter = 0f;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && _isGrounded)
        {
            _groundAttack = true;
            _playerAnimation.GroundAttackAnimation();
            StartCoroutine(GroundAttackRoutine());
        }
    }
    IEnumerator GroundAttackRoutine()
    {
        yield return new WaitForSeconds(1f);
        _groundAttack = false;
    }

    private void Movement()
    {
        _rigidBody2D.velocity = new Vector2(_horizontalMoveInput * _speed, _rigidBody2D.velocity.y);
    }

    private void Flip()
    {
        if (_horizontalMoveInput < 0 && !_isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingRight = !_isFacingRight;
        }
        else if (_horizontalMoveInput > 0 && _isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingRight = !_isFacingRight;
        }
    }

    private void HandleJump()
    {
        _playerAnimation.JumpAnimation(true);

        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);
        _rigidBody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        _jumpBufferTimeCounter = 0f;
    }

    private void UpdateJumpCounters()
    {
        if (_isGrounded && _rigidBody2D.velocity.y < 0)
            _playerAnimation.JumpAnimation(false);

        if (_isGrounded)
        {
            _cayoteTimeCounter = _cayoteTime;
            _playerAnimation.FallAnimation(false);
        }
        else
            _cayoteTimeCounter -= Time.deltaTime;

        _jumpBufferTimeCounter -= Time.deltaTime;
    }

    private void Gravity()
    {
        if (_rigidBody2D.velocity.y < -1 && (_cayoteTimeCounter < 0f || _jumpBufferTimeCounter < 0f))
        {
            _playerAnimation.FallAnimation(true);

            _rigidBody2D.gravityScale = _gravityScale * _fallMultiplier;            
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Max(_rigidBody2D.velocity.y, -_maxFallSpeed));
        }
        else
            _rigidBody2D.gravityScale = _gravityScale;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, _groundLayerMask);
    }
}