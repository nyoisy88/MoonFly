using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private static int currentLevel = 1;
    [SerializeField] private List<GameLevel> levelPrefabs;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private int score;
    private float time;
    private bool isTimerActive;
    private bool isGamePaused;

    public int CurrentScore => score;
    public float CurrentTime => time;
    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Rocket.Instance.OnCoinPickedUp += Rocket_OnCoinPickedUp;
        Rocket.Instance.OnLanded += Rocket_OnLanded;
        Rocket.Instance.OnStateChanged += Rocket_OnStateChanged;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        LoadGameLevel();
    }

    private void Update()
    {
        if (!isTimerActive) return;
        time += Time.deltaTime;
    }

    private void Rocket_OnStateChanged(object sender, Rocket.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Rocket.State.Normal;
        if (isTimerActive)
        {
            cinemachineCamera.Target.TrackingTarget = Rocket.Instance.transform;
            CameraOrthographicZoom2D.Instance.SetNormalOrthographicSize();
        }
    }    
    
    private void Rocket_OnLanded(object sender, Rocket.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Rocket_OnCoinPickedUp(object sender, EventArgs e)
    {
        AddScore(100);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void LoadGameLevel()
    {
        foreach (GameLevel gameLevel in levelPrefabs)
        {
            if (gameLevel.Level == currentLevel)
            {
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Rocket.Instance.transform.position =    spawnedGameLevel.RocketStartPosition;
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.CameraStartTargetTransfrom;
                CameraOrthographicZoom2D.Instance.TargetOrthographicSize = spawnedGameLevel.ZoomOutOrthographicSize;
                return;
            }
        }
    }


    private void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        Debug.Log($"Score: {score}");
    }

    public void Retry()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public void NextLevel()
    {
        currentLevel++;
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }
}
