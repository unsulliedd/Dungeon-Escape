using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidBody2D;
    private Vector2 _moveInput;

    private void Start()
    {
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Player's Rigidbody2D is null");
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
        _moveInput = context.ReadValue<Vector2>();
    }
}
