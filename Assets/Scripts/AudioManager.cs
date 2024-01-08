using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private bool _playBGMusic;
    [SerializeField] private float _minDistance;
    [SerializeField] private AudioSource[] _playerAudioSource;
    [SerializeField] private AudioSource[] _enemyMossGiantAudioSource;
    [SerializeField] private AudioSource[] _enemySkeletonAudioSource;
    [SerializeField] private AudioSource[] _enemySpiderAudioSource;
    [SerializeField] private AudioSource[] _trapAudioSource;
    [SerializeField] private AudioSource[] _waterfallAudioSorces;
    [SerializeField] private AudioSource[] _bgMusicAudioSource;
    [SerializeField] private AudioSource _clickSoundAudioSource;

    private Player _player;
    private int _bgMusicIndex;
    private bool _isGamePaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (_playBGMusic && !_bgMusicAudioSource[_bgMusicIndex].isPlaying && !_isGamePaused)
            PlayBGMusic(Random.Range(0, _bgMusicAudioSource.Length));      
    }

    public void PlayPlayerSounds(int playerSoundIndex)
    {
        if(playerSoundIndex < _playerAudioSource.Length)
            _playerAudioSource[playerSoundIndex].Play();
    }

    public void StopPlayerSounds(int playerSoundIndex)
    {
        if (playerSoundIndex < _playerAudioSource.Length)
            _playerAudioSource[playerSoundIndex].Stop();
    }
   
    public void PlayMossGiantSounds(int enemySoundIndex, Vector2 _enemyLocation)
    {
        if (enemySoundIndex < _enemyMossGiantAudioSource.Length)
        {
            _enemyMossGiantAudioSource[enemySoundIndex].transform.position = _enemyLocation;
            _enemyMossGiantAudioSource[enemySoundIndex].Play();
        }
    }

    public void PlaySkeletonSounds(int enemySoundIndex, Vector2 _enemyPosition)
    {
        if (enemySoundIndex < _enemySkeletonAudioSource.Length)
        {
            _enemySkeletonAudioSource[enemySoundIndex].transform.position = _enemyPosition;
            _enemySkeletonAudioSource[enemySoundIndex].Play();
        }
    }

    public void PlaySpiderSounds(int enemySoundIndex, Vector2 _enemyPosition)
    {
        if (enemySoundIndex < _enemySpiderAudioSource.Length)
        {
            _enemySpiderAudioSource[enemySoundIndex].transform.position = _enemyPosition;
            _enemySpiderAudioSource[enemySoundIndex].Play();
        }
    }

    public void PlayTrapSounds(int trapSoundIndex, Transform _trapLocation)
    {
        if(_trapLocation != null && Vector2.Distance(_player.transform.position, _trapLocation.position) > _minDistance)
            return;

        if (trapSoundIndex < _trapAudioSource.Length)
        {      
            _trapAudioSource[trapSoundIndex].transform.position = _trapLocation.position;
            _trapAudioSource[trapSoundIndex].Play();
        }
    }

    public void StopTrapSounds(int trapSoundIndex)
    {
        if (trapSoundIndex < _trapAudioSource.Length)
            _trapAudioSource[trapSoundIndex].Stop();
    }

    public void PlayBGMusic(int bgMusicIndex)
    {
        _bgMusicIndex = bgMusicIndex;
        if (bgMusicIndex < _bgMusicAudioSource.Length)
            _bgMusicAudioSource[bgMusicIndex].Play();
    }

    public void StopBGMusic()
    {
        for (int i =0; i < _bgMusicAudioSource.Length; i++)
            _bgMusicAudioSource[i].Stop();
    }

    public void PlayClickSound()
    {
        _clickSoundAudioSource.Play();
    }

    public void TogglePause()
    {
        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
            PauseAllSounds();
        else
            ResumeAllSounds();
    }

    private void PauseAllSounds()
    {
        foreach (var audioSource in _bgMusicAudioSource)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
        
        foreach (var audioSource in _trapAudioSource)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        foreach (var audioSource  in _waterfallAudioSorces)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    private void ResumeAllSounds()
    {
        foreach (var audioSource in _bgMusicAudioSource)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }

        foreach (var audioSource in _trapAudioSource)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }

        foreach (var audioSource in _waterfallAudioSorces)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }
    }
}