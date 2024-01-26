using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private AudioSource _audioSource;
    private Player _player;
    private bool _canDamage = true;

    private void Awake()
    {
        if(!TryGetComponent(out _audioSource))
            Debug.Log("Attack's AudioSource is null");
        if(!GameObject.FindWithTag("Player").TryGetComponent(out _player))
            Debug.Log("Attack's Player is null");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && _canDamage)
        {
            _audioSource.Play();
            if (_player._blocking)
                Instantiate(_player._blockFx, other.transform.position + new Vector3(0.35f,0,0), Quaternion.identity);
            else
                Instantiate(_player._attackFx, other.transform.position + new Vector3(0.2f, 0.2f, 0), Quaternion.identity);

            damageable.Damage();
            _canDamage = false;
            StartCoroutine(ResetDamage());
        }
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.75f);
        _canDamage = true;
    }
}
