using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public const float GRAVITY_NORMAL = 0.7f;
    public static Rocket Instance { get; private set; }

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int landingAngle;
        public int landingSpeed;
        public int scoreMultiplier;
        public int score;
    }

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum LandingType
    {
        WrongLandingArea,
        TooFastLanding,
        TooSteepAngle,
        Success,
    };

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public event EventHandler OnCoinPickedUp;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnUpForce;
    public event EventHandler<bool> OnTurnForce;

    private State state;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private float fuelExhaustRate = 1.0f;

    public Vector2 Speed => rb.linearVelocity;

    public State CurrentState
    {
        get => state;
        set
        {
            if (state == value) return;
            state = value;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = value,
            });
        }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        fuelAmount = fuelAmountMax;
        rb.gravityScale = 0f;
        state = State.WaitingToStart;
    }

    void Update()
    {

        moveDir = GameInput.Instance.GetMovementInput();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            case State.WaitingToStart:
                if (moveDir != Vector2.zero)
                {
                    ConsumeFuel();
                    rb.gravityScale = GRAVITY_NORMAL;
                    CurrentState = State.Normal;
                }
                break;
            case State.Normal:
                if (fuelAmount < 0f)
                {
                    return;
                }
                float gamePadDeadzone = .4f;
                if (moveDir != Vector2.zero)
                {
                    ConsumeFuel();
                }
                float force = 700f;
                if (moveDir.y > gamePadDeadzone)
                {
                    rb.AddForce(force * Time.fixedDeltaTime * transform.up);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                float turnSpeed = -100f;
                if (Mathf.Abs(moveDir.x) > gamePadDeadzone)
                {
                    rb.AddTorque(moveDir.x * Time.fixedDeltaTime * turnSpeed);
                    bool turnRight = moveDir.x > 0f;
                    OnTurnForce?.Invoke(this, turnRight);
                }
                break;
            case State.GameOver:
                break;
        }


    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    private void ConsumeFuel()
    {
        fuelAmount -= fuelExhaustRate * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrentState = State.GameOver;
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                score = 0,
            });
            return;
        }
        float softLandingVelocityMagtitude = 5f;
        float landingSpeedMagtitude = collision.relativeVelocity.magnitude;
        if (landingSpeedMagtitude > softLandingVelocityMagtitude)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude),
            });
            return;
        }

        float dotVectorLanding = Vector2.Dot(Vector2.up, transform.up);
        float minDotLandingVector = 0.9f;
        if (dotVectorLanding < minDotLandingVector)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
                score = 0,
            });
            return;
        }

        float landingSpeedScoreMultiplier = 100f;
        float landingSpeedScore = (softLandingVelocityMagtitude - landingSpeedMagtitude) * landingSpeedScoreMultiplier;

        float maxLandingAngleScore = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxLandingAngleScore - (1f - dotVectorLanding) * scoreDotVectorMultiplier * maxLandingAngleScore;
        int total = Mathf.RoundToInt(landingPad.ScoreMultiplier * (landingAngleScore + landingSpeedScore));
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
            landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude * 5),
            scoreMultiplier = landingPad.ScoreMultiplier,
            score = total
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Fuel fuel))
        {
            fuelAmount = fuelAmountMax;
            fuel.DestroySelf();
            return;
        }
        if (collision.gameObject.TryGetComponent(out Coin coin))
        {
            OnCoinPickedUp?.Invoke(this, EventArgs.Empty);
            coin.DestroySelf();
            return;
        }

    }
}
