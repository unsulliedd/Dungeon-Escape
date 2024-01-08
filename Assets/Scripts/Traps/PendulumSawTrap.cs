using UnityEngine;

public class PendulumSawTrap : TrapDamage
{
    [SerializeField] private float _swingForce;
    [SerializeField] private float _leftRange;
    [SerializeField] private float _rightRange;

    private Rigidbody2D _rigidBody2D;

    public override void Awake()
    {
        base.Awake();
        if (!TryGetComponent(out _rigidBody2D))
            Debug.Log("Pendulum's Rigidbody2D is null");
    }

    void Start()
    {
        // Randomize the direction of the swing
        _rigidBody2D.angularVelocity = Random.Range(0, 2) == 0 ? _swingForce : -_swingForce;
    }

    void Update()
    {
        Swing();
    }

    private void Swing()
    {
        // Check if the object's rotation along the Z-axis is positive,
        // within the specified right range, and the angular velocity is positive.
        if (transform.rotation.z > 0f
            && transform.rotation.z < _rightRange
            && _rigidBody2D.angularVelocity > 0
            && _rigidBody2D.angularVelocity < _swingForce)
        {
            // If the conditions are met, set the angular velocity to the specified swing force.
            _rigidBody2D.angularVelocity = _swingForce;
        }

        // Check if the object's rotation along the Z-axis is negative,
        // within the specified left range, and the angular velocity is negative.
        else if (transform.rotation.z < 0f
            && transform.rotation.z > _leftRange
            && _rigidBody2D.angularVelocity < 0
            && _rigidBody2D.angularVelocity < _swingForce * -1)
        {
            // If the conditions are met, set the angular velocity to the negative of the specified swing force.
            _rigidBody2D.angularVelocity = _swingForce * -1;
        }

        // Play the trap sound if the angular velocity is greater than 50 or less than -50.
        if (_rigidBody2D.angularVelocity > 50f || _rigidBody2D.angularVelocity < -50f)
            AudioManager.Instance.PlayTrapSounds(0, transform);
    }
}

