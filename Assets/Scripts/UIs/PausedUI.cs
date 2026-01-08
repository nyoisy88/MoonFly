using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button mainmenuBtn;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePause();

        });

        mainmenuBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        } );

        musicSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.MusicVolume = value;
        });

        soundSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.SoundVolume = value;
        });
    }

    private void Start()
    {
        musicSlider.value = MusicManager.Instance.MusicVolume;
        soundSlider.value = SoundManager.Instance.SoundVolume;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        container.gameObject.SetActive(false);
        resumeBtn.Select();
    }

    private void Show()
    {
        container.gameObject.SetActive(true);
    }
}
