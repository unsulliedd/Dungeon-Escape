using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private int _diamondValue = 1;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_player != null && other.CompareTag("Player"))
        {
            _player.Diamonds += _diamondValue;
            Destroy(gameObject);
        }
    }
}
