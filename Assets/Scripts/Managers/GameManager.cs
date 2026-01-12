using Signals;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using static Rocket;

public class GameManager : Singleton<GameManager>
{
    public const int COIN_PICKUP_SCORE = 500;
    public const int CARGO_DELIVERY_SCORE = 5000;


    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private static int currentLevel = 1;
    private static int totalScore = 0;

    public static void ResetStaticData()
    {
        totalScore = 0;
        currentLevel = 1;
    }

    [SerializeField] private List<GameLevel> levelPrefabs;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private State state;
    private int score;
    private float time;
    private bool isTimerActive;
    private bool isGamePaused;
    public int Score => score;
    public float Timer => time;
    public int CurrentLevel => currentLevel;

    public int TotalScore => totalScore;

    private void Start()
    {
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);
        Rocket.Instance.OnCoinPickedUp += Rocket_OnCoinPickedUp;
        Rocket.Instance.OnCargoDelivered += Rocket_OnCargoDropOff;
        Rocket.Instance.OnStateChanged += Rocket_OnStateChanged;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        LoadGameLevel();
    }

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(OnRocketLanded);
    }

    private void Update()
    {
        if (!isTimerActive) return;
        time += UnityEngine.Time.deltaTime;
    }

    private void Rocket_OnStateChanged(object sender, Rocket.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Rocket.State.Active;
        if (isTimerActive)
        {
            cinemachineCamera.Target.TrackingTarget = Rocket.Instance.transform;
            CameraOrthographicZoom2D.Instance.SetNormalOrthographicSize();
        }
    }

    private void OnRocketLanded(RocketLandedSignal signal)
    {
        AddScore(signal.score);
    }

    private void Rocket_OnCoinPickedUp(object sender, EventArgs e)
    {
        AddScore(COIN_PICKUP_SCORE);
    }

    private void Rocket_OnCargoDropOff(object sender, EventArgs e)
    {
        AddScore(CARGO_DELIVERY_SCORE);
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
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Rocket.Instance.transform.position = spawnedGameLevel.RocketStartPosition;
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.CameraStartTargetTransfrom;
        CameraOrthographicZoom2D.Instance.TargetOrthographicSize = spawnedGameLevel.ZoomOutOrthographicSize;
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in levelPrefabs)
        {
            if (gameLevel.Level == currentLevel)
            {
                return gameLevel;
            }
        }
        return null;
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
        totalScore += score;
        if (GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }
}
