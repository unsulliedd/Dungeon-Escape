using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    private Rigidbody2D _rigidBody2D;
    private Vector2 _moveInput;

    void Awake()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.green);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        // Update the horizontal (x-axis) velocity of the Rigidbody2D.
        // The vertical (y-axis) velocity remains unchanged to preserve any existing vertical motion like gravity or jumping.
        _rigidBody2D.velocity = new Vector2(_moveInput.x * _speed, _rigidBody2D.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log(IsGrounded());
        Debug.Log(context.phase);
        if (IsGrounded() && context.started)
        {
            // If the jump button is pressed, add an upward force to the Rigidbody2D.
            _rigidBody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.1f , 1 << 8);
    }
}