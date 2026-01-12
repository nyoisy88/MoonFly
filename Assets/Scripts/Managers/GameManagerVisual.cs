using Signals;
using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : Singleton<GameManagerVisual>
{   
    [SerializeField] protected CinemachineImpulseSource impulseSource;
    [SerializeField] private ScorePopup scorePopupPrefab;
    [SerializeField] private Transform pickupCollectVfx;

    private void Start()
    {
        //Rocket.Instance.OnLanded += Rocket_OnLanded;
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);
        Rocket.Instance.OnCoinPickedUp += Rocket_OnCoinPickedUp;
        Rocket.Instance.OnFuelPickedUp += Rocket_OnFuelPickedUp;
        Rocket.Instance.OnCargoDelivered += Rocket_OnCargoDelivered;
    }

    private void Rocket_OnCargoDelivered(object sender, Rocket.OnCargoDeliveredEventArgs e)
    {
        PlayPickupFeedback($"+{GameManager.CARGO_DELIVERY_SCORE}", e.cargo.transform.position);
    }

    private void Rocket_OnFuelPickedUp(object sender, Rocket.OnFuelPickedUpEventArgs e)
    {
        PlayPickupFeedback("REFILL", e.fuelPickup.transform.position);
    }

    private void Rocket_OnCoinPickedUp(object sender, Rocket.OnCoinPickedUpEventArgs e)
    {
        PlayPickupFeedback($"+{GameManager.COIN_PICKUP_SCORE}", e.coinPickup.transform.position);
    }

    private void OnRocketLanded(RocketLandedSignal signal)
    {
        switch (signal.landingType)
        {
            case Rocket.LandingType.TooSteepAngle:
            case Rocket.LandingType.TooFastLanding:
            case Rocket.LandingType.WrongLandingArea:
                impulseSource.GenerateImpulse();
                break;
        }
    }

    private void PlayPickupFeedback(string message, Vector3 position)
    {
        ScorePopup scorePopup = Instantiate(scorePopupPrefab, position, Quaternion.identity);
        scorePopup.SetText(message);
        Transform pickupCollectTransform = Instantiate(pickupCollectVfx, position, Quaternion.identity);
        Destroy(pickupCollectTransform.gameObject, 1f);
    }

    private void OnDestroy()
    {
        SignalBus.Unsubcribe<RocketLandedSignal>(OnRocketLanded);
    }
}
