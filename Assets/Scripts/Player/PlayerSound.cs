using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _runSound;

    void Awake()
    {
        if (!GetComponentInChildren<AudioSource>())
            Debug.Log("Player's AudioSource is null");
        else
            _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void RunSound()
    {
        if (!_audioSource.isPlaying || _audioSource.clip != _runSound)
        {
            _audioSource.clip = _runSound;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    public void StopRunSound()
    {
        if (_audioSource.clip == _runSound)
            _audioSource.Stop();
    }
}