using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public const string PLAYER_PREFS_SOUND_VOLUME = "Sound_Volume";

    public event EventHandler<float> OnSoundVolumeChanged;

    [SerializeField] private AudioClip coinPickUp;
    [SerializeField] private AudioClip fuelPickUp;
    [SerializeField] private AudioClip crash;
    [SerializeField] private AudioClip landingSuccess;

    private float soundVolumeMultiplier = 1f;

    public float SoundVolume
    {
        get => soundVolumeMultiplier;
        set
        {
            value = Mathf.Clamp01(value);
            if (Mathf.Approximately(soundVolumeMultiplier, value))
                return;
            soundVolumeMultiplier = value;
            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, soundVolumeMultiplier);
            OnSoundVolumeChanged?.Invoke(this, soundVolumeMultiplier);
        }
    }

    private void Start()
    {
        soundVolumeMultiplier = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, 1f);
        Rocket.Instance.OnCoinPickedUp += Rocket_OnCoinPickedUp;
        Rocket.Instance.OnFuelPickedUp += Rocket_OnFuelPickedUp;
        Rocket.Instance.OnLanded += Rocket_OnLanded;
    }

    private void Rocket_OnLanded(object sender, Rocket.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Rocket.LandingType.Success:
                PlaySound(landingSuccess);
                break;
            default:
                PlaySound(crash); 
                break;
        }
    }

    private void Rocket_OnFuelPickedUp(object sender, System.EventArgs e)
    {
        PlaySound(fuelPickUp);
    }

    private void Rocket_OnCoinPickedUp(object sender, System.EventArgs e)
    {
        PlaySound(coinPickUp);
    }

    private void PlaySound(AudioClip[] clips, float volume = 1f)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        PlaySound(clips[randomIndex], volume);
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume * soundVolumeMultiplier);
    }
}
