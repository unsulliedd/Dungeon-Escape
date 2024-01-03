using UnityEngine;

public class MovingSawTrap : TrapDamage
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    private Vector3 _currentTarget;

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Check if the current position of the trap.
        // If true, set the current target to be the end point.
        if (transform.position == _startPoint.position)     
            _currentTarget = _endPoint.position;
        else if (transform.position == _endPoint.position)
            _currentTarget = _startPoint.position;

        // Move the trap towards the current target using linear interpolation.
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _speed * Time.deltaTime);

        // Rotate the trap around its forward axis (Z-axis) with a specified turn speed.
        transform.Rotate(Vector3.forward, _turnSpeed * Time.deltaTime);
    }
}
