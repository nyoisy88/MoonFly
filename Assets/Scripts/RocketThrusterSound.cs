using UnityEngine;

public class RocketThrusterSound : MonoBehaviour
{
    [SerializeField] private Rocket rocket;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnMusicVolumeChanged;
        GameManager.Instance.OnGamePaused += GM_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        audioSource.enabled = true;
    }

    private void GM_OnGamePaused(object sender, System.EventArgs e)
    {
        audioSource.enabled = false;
    }

    private void SoundManager_OnMusicVolumeChanged(object sender, float soundVolume)
    {
        audioSource.volume = soundVolume;
    }

    private void Update()
    {
        if (!audioSource.enabled) return;
        if (rocket.IsMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
}
