using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Slider")]
    [SerializeField] private Slider menuVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider environmentVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider changeFOVSlider;

    [Header("UI Toggle")]
    [SerializeField] private Toggle muteVolumeToggle;
    [SerializeField] private Toggle stopMusicToggle;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI menuVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI environmentVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI fovText;

    // Private Variables
    private int muteValue;
    private int stopMusic;
    private readonly float minVolume = -80f;
    private readonly float maxVolume = 0f;

    // Player Prefs Keys
    private const string MenuVolumeKey = "MenuVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string EnvironmentVolumeKey = "EnvironmentVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string MuteVolumeKey = "MuteVolume";
    private const string StopMusicKey = "StopMusic";
    private const string FOVKey = "FOV";

    void Start()
    {
        // Set the initial values to the loaded values from PlayerPrefs
        menuVolumeSlider.value = PlayerPrefs.GetFloat(MenuVolumeKey, 0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, 0f);
        environmentVolumeSlider.value = PlayerPrefs.GetFloat(EnvironmentVolumeKey, 0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0f);
        muteVolumeToggle.isOn = PlayerPrefs.GetInt(MuteVolumeKey, 0) == 1;
        stopMusicToggle.isOn = PlayerPrefs.GetInt(StopMusicKey, 0) == 1;
        changeFOVSlider.value = PlayerPrefs.GetFloat(FOVKey, 55f);

        // Check if music should be stopped, and adjust volume accordingly
        if (stopMusicToggle.isOn && AudioManager.Instance != null)
            AudioManager.Instance._playBGMusic = false;
        if (muteVolumeToggle.isOn)
            audioMixer.SetFloat("Master", -80f);
    }

    // Set the volume for the Menu Audio Mixer group and update UI elements
    public void SetMenuVolume()
    {
        audioMixer.SetFloat("Menu", menuVolumeSlider.value);
        UpdateSliderElements(menuVolumeSlider.value, menuVolumeText, menuVolumeSlider);
    }

    // Set the volume for the SFX Audio Mixer group and update UI elements
    public void SetSfxVolume()
    {
        audioMixer.SetFloat("SFX", sfxVolumeSlider.value);
        UpdateSliderElements(sfxVolumeSlider.value, sfxVolumeText, sfxVolumeSlider);
    }

    // Set the volume for the Environment Audio Mixer group and update UI elements
    public void SetEnvironmentVolume()
    {
        audioMixer.SetFloat("Environment", environmentVolumeSlider.value);
        UpdateSliderElements(environmentVolumeSlider.value, environmentVolumeText, environmentVolumeSlider);
    }

    // Set the volume for the Music Audio Mixer group and update UI elements
    public void SetMusicVolume()
    {
        audioMixer.SetFloat("Music", musicVolumeSlider.value);
        UpdateSliderElements(musicVolumeSlider.value, musicVolumeText, musicVolumeSlider);
    }

    // Set the Field of View (FOV) for the game
    public void SetFOV()
    {
        // Clamp FOV value to a range and update UI text
        float clampedFOV = Mathf.Clamp(changeFOVSlider.value, 30f, 60f);
        fovText.text = Mathf.Round(clampedFOV).ToString();

        // If GameManager is available, update FOV in the game
        if (GameManager.Instance != null)
            GameManager.Instance.ChangeFOV(clampedFOV);
    }

    // Update UI elements based on the slider value
    private void UpdateSliderElements(float sliderValue, TextMeshProUGUI text, Slider slider)
    {
        float normalizedValue = Mathf.InverseLerp(minVolume, maxVolume, sliderValue) * 100f;
        text.text = Mathf.Round(normalizedValue).ToString();
    }

    // Toggle mute for the entire volume
    public void MuteVolumeToggle()
    {
        // Set mute value based on toggle state and adjust the volume accordingly
        muteValue = muteVolumeToggle.isOn ? 1 : 0;
        audioMixer.SetFloat("Master", muteVolumeToggle.isOn ? -80f : 0f);
    }

    // Toggle stopping background music
    public void StopMusicToggle()
    {
        // Set stopMusic value based on toggle state and stop or resume background music
        stopMusic = stopMusicToggle.isOn ? 1 : 0;
        if (GameManager.Instance != null)
        {
            AudioManager.Instance.StopBGMusic();
            AudioManager.Instance._playBGMusic = !stopMusicToggle.isOn;
        }
        else
            audioMixer.SetFloat("Menu", stopMusicToggle.isOn ? -80f : 0f);
    }

    // Reset all settings to default values
    public void ResetSettings()
    {
        menuVolumeSlider.value = 1f;
        sfxVolumeSlider.value = 1f;
        environmentVolumeSlider.value = 1f;
        musicVolumeSlider.value = 1f;
        muteVolumeToggle.isOn = false;
        stopMusicToggle.isOn = false;
        changeFOVSlider.value = 55f;

        // Apply mute setting and save all settings
        MuteVolumeToggle();
        StopMusicToggle();
        SaveSettings();
    }

    // Save all current settings to PlayerPrefs
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MenuVolumeKey, menuVolumeSlider.value);
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(EnvironmentVolumeKey, environmentVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolumeSlider.value);
        PlayerPrefs.SetInt(MuteVolumeKey, muteValue);
        PlayerPrefs.SetInt(StopMusicKey, stopMusic);
        PlayerPrefs.SetFloat(FOVKey, changeFOVSlider.value);
    }
}
