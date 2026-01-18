using Signals;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using static Rocket;

public class GameManager : Singleton<GameManager>
{
    public const string PLAYER_PREFS_CURRENT_LEVEL = "Game_Level";
    public const int COIN_PICKUP_SCORE = 100;
    public const int CARGO_DELIVERY_SCORE = 1000;


    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private int currentLevel;
    private static int totalScore = 0;

    public static void ResetStaticData()
    {
        totalScore = 0;
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

    protected override void Awake()
    {
        base.Awake();
        currentLevel = 
             PlayerPrefs.GetInt(PLAYER_PREFS_CURRENT_LEVEL, 1);
    }
    private void Start()
    {
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);
        SignalBus.Subcribe<CoinPickedUpSignal>(OnCoinPickedUp);
        //SignalBus.Subcribe<CargoDeliveredSignal>(DeliverCargo);
        //Rocket.Instance.OnCargoDelivered += Rocket_OnCargoDropOff;
        Rocket.Instance.OnStateChanged += Rocket_OnStateChanged;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        LoadGameLevel();
    }

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(OnRocketLanded);
        SignalBus.Unsubcribe<CoinPickedUpSignal>(OnCoinPickedUp);
        //SignalBus.Unsubcribe<CargoDeliveredSignal>(DeliverCargo);

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

    public void DeliverCargo()
    {
        AddScore(CARGO_DELIVERY_SCORE);
    }

    private void OnRocketLanded(RocketLandedSignal signal)
    {
        if (signal.landingType == LandingType.Success)
        {
            AddScore(signal.score);
        }
    }

    private void OnCoinPickedUp(CoinPickedUpSignal signal)
    {
        AddScore(COIN_PICKUP_SCORE);
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
            PlayerPrefs.SetInt(PLAYER_PREFS_CURRENT_LEVEL, 1);
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_CURRENT_LEVEL, currentLevel);
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }
}
