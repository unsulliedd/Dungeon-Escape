using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _canDamage = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && _canDamage)
        {
            _audioSource.Play();
            damageable.Damage();
            _canDamage = false;
            StartCoroutine(ResetDamage());
        }
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(1.05f);
        _canDamage = true;
    }
}
