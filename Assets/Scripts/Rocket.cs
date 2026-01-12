using Signals;
using System;
using UnityEngine;

public class Rocket : Singleton<Rocket>
{
    public const float GRAVITY_NORMAL = 0.7f;


    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public class OnFuelPickedUpEventArgs : EventArgs
    {
        public Fuel fuelPickup;
    }

    public class OnCoinPickedUpEventArgs : EventArgs
    {
        public Coin coinPickup;
    }

    public class OnCargoDeliveredEventArgs : EventArgs
    {
        public CargoChainCrate cargo;
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
        WaitingForInput,
        Active,
        Disabled,
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    //public event EventHandler<OnLandedEventArgs> OnLanded;
    public event EventHandler<OnFuelPickedUpEventArgs> OnFuelPickedUp;
    public event EventHandler<OnCoinPickedUpEventArgs> OnCoinPickedUp;
    public event EventHandler<OnCargoDeliveredEventArgs> OnCargoDelivered;
    
    public event EventHandler OnBeforeForce;
    public event EventHandler OnUpForce;
    public event EventHandler<bool> OnTurnForce;

    [SerializeField] private GameObject cargoChainPrefab;

    private State state;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private float fuelAmount;
    private bool isMoving;
    private CargoChainCrate cargo;
    private CargoChain chain;

    private readonly float fuelAmountMax = 12f;
    private readonly float fuelExhaustRate = 1f;
    readonly float gamePadDeadzone = .4f;

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

    public bool IsMoving => isMoving;

    public CargoChainCrate Cargo => cargo;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        fuelAmount = fuelAmountMax;
        rb.gravityScale = 0f;
        state = State.WaitingForInput;
    }

    void Update()
    {
        moveDir = GameInput.Instance.GetMovementInput();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        isMoving = false;

        switch (state)
        {
            case State.WaitingForInput:
                if (moveDir.y > gamePadDeadzone || Mathf.Abs(moveDir.x) > gamePadDeadzone)
                {
                    ConsumeFuel();
                    rb.gravityScale = GRAVITY_NORMAL;
                    CurrentState = State.Active;
                    isMoving = true;
                }
                break;
            case State.Active:
                if (fuelAmount < 0f)
                {
                    return;
                }
                if (moveDir.y > gamePadDeadzone || Mathf.Abs(moveDir.x) > gamePadDeadzone)
                {
                    ConsumeFuel();
                    isMoving = true;
                }
                float force = 7000f;
                if (moveDir.y > gamePadDeadzone)
                {
                    rb.AddForce(force * Time.fixedDeltaTime * transform.up);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                float turnSpeed = -1000f;
                if (Mathf.Abs(moveDir.x) > gamePadDeadzone)
                {
                    rb.AddTorque(moveDir.x * Time.fixedDeltaTime * turnSpeed);
                    bool turnRight = moveDir.x > 0f;
                    OnTurnForce?.Invoke(this, turnRight);
                }
                break;
            case State.Disabled:
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

    public void AddFuel(Fuel fuelPickup)
    {
        fuelAmount = fuelAmountMax;
        OnFuelPickedUp?.Invoke(this, new OnFuelPickedUpEventArgs
        {
            fuelPickup = fuelPickup
        });
    }

    public void AddCoin(Coin coinPickup)
    {
        OnCoinPickedUp?.Invoke(this, new OnCoinPickedUpEventArgs
        {
            coinPickup = coinPickup
        });
    }

    public void PickUpCargo(CargoChainCrate cargo)
    {
        if (this.cargo != null) return;
        CargoChain cargoChain = Instantiate(cargoChainPrefab, transform).GetComponent<CargoChain>();
        cargoChain.Attach(rb, cargo);
        this.chain = cargoChain;
        this.cargo = cargo;
    }

    private void ClearCargo()
    {
        cargo = null;
    }

    public bool HasCargo()
    {
        return cargo != null;
    }

    public void DeliverCargo()
    {
        if (cargo != null)
        {
            OnCargoDelivered?.Invoke(this, new OnCargoDeliveredEventArgs
            {
                cargo = this.cargo,
            });
            Destroy(cargo.gameObject);
            Destroy(chain.gameObject);
            ClearCargo();
            ClearChain();
        }
    }

    public void CargoCrashed()
    {
        Destroy(cargo.gameObject);
        Destroy(chain.gameObject);
        ClearCargo();
        ClearChain();
    }

    private void ClearChain()
    {
        chain = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrentState = State.Disabled;
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            SignalBus.Fire(new RocketLandedSignal
            {
                landingType = LandingType.WrongLandingArea,
                score = 0,
            });
            //OnLanded?.Invoke(this, new OnLandedEventArgs
            //{
            //    landingType = LandingType.WrongLandingArea,
            //    score = 0,
            //});
            return;
        }
        float softLandingVelocityMagtitude = 5f;
        float landingSpeedMagtitude = collision.relativeVelocity.magnitude;
        if (landingSpeedMagtitude > softLandingVelocityMagtitude)
        {
            SignalBus.Fire(new RocketLandedSignal
            {
                landingType = LandingType.TooFastLanding,
                landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude * 5),
            });
            //OnLanded?.Invoke(this, new OnLandedEventArgs
            //{
            //    landingType = LandingType.TooFastLanding,
            //    landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude * 5),
            //});
            return;
        }

        float dotVectorLanding = Vector2.Dot(Vector2.up, transform.up);
        float minDotLandingVector = 0.9f;
        if (dotVectorLanding < minDotLandingVector)
        {
            SignalBus.Fire(new RocketLandedSignal
            {
                landingType = LandingType.TooSteepAngle,
                landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
            });
            //OnLanded?.Invoke(this, new OnLandedEventArgs
            //{
            //    landingType = LandingType.TooSteepAngle,
            //    landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
            //    score = 0,
            //});
            return;
        }

        float landingSpeedScoreMultiplier = 100f;
        float landingSpeedScore = (softLandingVelocityMagtitude - landingSpeedMagtitude) * landingSpeedScoreMultiplier;

        float maxLandingAngleScore = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxLandingAngleScore - (1f - dotVectorLanding) * scoreDotVectorMultiplier * maxLandingAngleScore;
        int total = Mathf.RoundToInt(landingPad.ScoreMultiplier * (landingAngleScore + landingSpeedScore));

        SignalBus.Fire(new RocketLandedSignal
        {
            landingType = LandingType.Success,
            landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
            landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude * 5),
            scoreMultiplier = landingPad.ScoreMultiplier,
            score = total
        });
        //OnLanded?.Invoke(this, new OnLandedEventArgs
        //{
        //    landingType = LandingType.Success,
        //    landingAngle = Mathf.RoundToInt(dotVectorLanding * 100),
        //    landingSpeed = Mathf.RoundToInt(landingSpeedMagtitude * 5),
        //    scoreMultiplier = landingPad.ScoreMultiplier,
        //    score = total
        //});
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPickup pickups))
        {
            pickups.OnCollected(this);
        }

    }

}
