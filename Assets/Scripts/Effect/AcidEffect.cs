using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    private GameObject _player;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Vector3 direction = _player.transform.position - transform.position;
        _rigidbody2D.velocity = direction.normalized * _speed;

        Destroy(this.gameObject, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out var hit))
            {
                hit.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
