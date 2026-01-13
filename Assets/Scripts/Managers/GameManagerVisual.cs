using Signals;
using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : Singleton<GameManagerVisual>
{
    [SerializeField] protected CinemachineImpulseSource impulseSource;
    [SerializeField] private ScorePopup scorePopupPrefab;
    [SerializeField] private Transform pickupCollectVfx;
    [SerializeField] private GameObject cargoCrashVfx;


    private void Start()
    {
        //Rocket.Instance.OnLanded += Rocket_OnLanded;
        SignalBus.Subcribe<RocketLandedSignal>(OnRocketLanded);
        SignalBus.Subcribe<CargoCrashedSignal>(CargoChainCrate_OnCargoCrashed);
        SignalBus.Subcribe<FuelPickedUpSignal>(OnFuelPickedUp);
        SignalBus.Subcribe<CoinPickedUpSignal>(OnCoinPickedUp);

        //Rocket.Instance.OnCargoDelivered += Rocket_OnCargoDelivered;
    }

    private void CargoChainCrate_OnCargoCrashed(CargoCrashedSignal signal)
    {
        GameObject cargoCrashVfxGO = Instantiate(cargoCrashVfx, (Vector3)signal.crashPoint, Quaternion.identity);
        Destroy(cargoCrashVfxGO, 1f);
    }

    private void OnFuelPickedUp(FuelPickedUpSignal signal)
    {
        PlayPickupFeedback("REFILL", signal.pickupPosition);
    }

    private void OnCoinPickedUp(CoinPickedUpSignal signal)
    {
        PlayPickupFeedback($"+{GameManager.COIN_PICKUP_SCORE}", signal.pickupPosition);
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
        SignalBus.Unsubcribe<CargoCrashedSignal>(CargoChainCrate_OnCargoCrashed);
        SignalBus.Unsubcribe<CoinPickedUpSignal>(OnCoinPickedUp);
        SignalBus.Unsubcribe<FuelPickedUpSignal>(OnFuelPickedUp);
    }
}
