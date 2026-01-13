using Signals;
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

    protected override void Awake()
    {
        base.Awake();
        soundVolumeMultiplier = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, 1f);
    }

    private void Start()
    {
        SignalBus.Subcribe<CoinPickedUpSignal>(OnCoinPickedUp);
        SignalBus.Subcribe<FuelPickedUpSignal>(OnFuelPickedUp);
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);
        // Rocket.Instance.OnCargoDelivered += Rocket_OnCargoDelivered;
    }

    private void OnRocketLanded(RocketLandedSignal signal)
    {
        switch (signal.landingType)
        {
            case Rocket.LandingType.Success:
                PlaySound(landingSuccess);
                break;
            default:
                PlaySound(crash);
                break;
        }
    }

    private void OnFuelPickedUp(FuelPickedUpSignal signal)
    {
        PlaySound(fuelPickUp);
    }

    private void OnCoinPickedUp(CoinPickedUpSignal signal)
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

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(OnRocketLanded);
        SignalBus.Unsubcribe<CoinPickedUpSignal>(OnCoinPickedUp);
        SignalBus.Unsubcribe<FuelPickedUpSignal>(OnFuelPickedUp);

    }
}
