using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpForce = 16f;
    [SerializeField] private bool _isRunning;
    private float _horizontalMoveInput;
    public bool _isFacingLeft;

    [Header("Jump Settings")]
    [SerializeField] private float _cayoteTime = 0.1f;
    [SerializeField] private float _cayoteTimeCounter;
    [SerializeField] private float _jumpBufferTime = 0.1f;
    [SerializeField] private float _jumpBufferTimeCounter;
    [SerializeField] private bool _canSecondJump;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckLength = 0.5f;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _spikeLayerMask;

    [Header("Gravity Settings")]
    [SerializeField] private float _gravityScale = 2f;
    [SerializeField] private float _maxFallSpeed = 16f;
    [SerializeField] private float _fallMultiplier = 2f;

    [Header("Attack Settings")]
    [SerializeField] private bool _groundAttack;
    [SerializeField] private bool _blocking;

    [Header("References")]
    private Rigidbody2D _rigidBody2D;
    private PlayerAnimation _playerAnimation;
    private PlayerSound _playerSound;

    [Header("Properties")]
    [SerializeField] private bool isPlayerAlive;
    public int Diamonds;

    public int Health { get; set; }

    void Awake()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
        if (!TryGetComponent(out _playerAnimation))
            Debug.Log("Player's PlayerAnimation is null");
        if (!TryGetComponent(out _playerSound))
            Debug.Log("Player's PlayerSound is null");
        Health = 4;
        isPlayerAlive = true;
    }

    void FixedUpdate()
    {
        if (!IsPlayerAlive())
            return;
        _isGrounded = IsGrounded();

        if (OnSpike())
            InstantDeath();

        Movement();
        Gravity();
    }

    void Update()
    {
        if (!IsPlayerAlive())
            return;

        Flip();
        UpdateJumpCounters();
        FootStep();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // prevent movement while blocking and attacking
        if (_blocking || _groundAttack || !IsPlayerAlive())
            return;

        _horizontalMoveInput = context.ReadValue<Vector2>().x;

        if (_horizontalMoveInput != 0 && _isGrounded)
            _isRunning = true;            
        else
            _isRunning = false;

        _playerAnimation.RunAnimation(_horizontalMoveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsPlayerAlive())
            return;

        if (context.started)
            _jumpBufferTimeCounter = _jumpBufferTime;
        else if (context.performed)
        {
            if (_cayoteTimeCounter > 0f && _jumpBufferTimeCounter > 0f && !_groundAttack && !_blocking)
            {                
                _canSecondJump = true;
                HandleJump();
            }
            else if (_canSecondJump && _rigidBody2D.velocity.y > 0f && !_groundAttack && !_blocking)
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
        if (context.performed && _isGrounded && !_groundAttack)
        {
            // prevent attacking while running
            if (_isRunning)
                return;

            _groundAttack = true;
            _playerAnimation.GroundAttackAnimation();
            AudioManager.Instance.PlayPlayerSounds(2);
            StartCoroutine(GroundAttackRoutine());
        }
    }
    IEnumerator GroundAttackRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        _groundAttack = false;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isGrounded)
            {
                _playerAnimation.BlockAnimation(true);
                _blocking = true;
            }
        }
        else if (context.canceled)
        {
            _playerAnimation.BlockAnimation(false);
            _blocking = false;
        }
    }

    private void Movement()
    {
        _rigidBody2D.velocity = new Vector2(_horizontalMoveInput * _speed, _rigidBody2D.velocity.y);
    }

    private void FootStep()
    {
        if (_rigidBody2D.velocity.y < 0.5 && _isRunning && _isGrounded)
            _playerSound.RunSound();
        else
            _playerSound.StopRunSound();
    }

    private void Flip()
    {
        if (_horizontalMoveInput < 0 && !_isFacingLeft)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingLeft = !_isFacingLeft;
        }
        else if (_horizontalMoveInput > 0 && _isFacingLeft)
        {
            transform.Rotate(0f, 180f, 0f);
            _isFacingLeft = !_isFacingLeft;
        }
    }

    private void HandleJump()
    {
        _playerAnimation.JumpAnimation(true);
        AudioManager.Instance.PlayPlayerSounds(0);
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);
        _rigidBody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        _jumpBufferTimeCounter = 0f;
    }

    private void UpdateJumpCounters()
    {
        if (_isGrounded && _rigidBody2D.velocity.y < 0.5)
        {
            _playerAnimation.JumpAnimation(false);
            _playerAnimation.FallAnimation(false);
        }

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
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Max(_rigidBody2D.velocity.y, - _maxFallSpeed));
            if (_isGrounded)
               AudioManager.Instance.PlayPlayerSounds(1);
        }
        else
            _rigidBody2D.gravityScale = _gravityScale;
    }

    private void OnDeath()
    {
        _rigidBody2D.velocity = Vector2.zero;
        isPlayerAlive = false;
        AudioManager.Instance.PlayPlayerSounds(4);
        _playerAnimation.DeathAnimation();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckLength, _groundLayerMask);
    }

    private bool OnSpike()
    {
        return Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckLength, _spikeLayerMask);
    }

    public void InstantDeath()
    {
        Health = 0;
        UIManager.Instance.UpdateHealth(Health);
        OnDeath();
    }

    public void Damage()
    {
        if (Health < 1)
            return;
        if (_blocking)
            return;
        Health--;
        AudioManager.Instance.PlayPlayerSounds(3);
        UIManager.Instance.UpdateHealth(Health);
        if (Health < 1)
            OnDeath();
    }

    public void AddDiamonds(int amount)
    {
        Diamonds += amount;
        GameManager.Instance.collectedDiamondCount++;
        AudioManager.Instance.PlayPlayerSounds(5);
        UIManager.Instance.UpdatePlayerDiamondCount(Diamonds);
    }

    public bool IsPlayerAlive()
    {
        return isPlayerAlive;
    }
}