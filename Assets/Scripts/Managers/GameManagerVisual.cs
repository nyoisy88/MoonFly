using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : Singleton<GameManagerVisual>
{   
    [SerializeField] protected CinemachineImpulseSource impulseSource;
    [SerializeField] private ScorePopup scorePopupPrefab;
    [SerializeField] private Transform pickupCollectVfx;

    private void Start()
    {
        Rocket.Instance.OnLanded += Rocket_OnLanded;
        Rocket.Instance.OnCoinPickedUp += Rocket_OnCoinPickedUp;
        Rocket.Instance.OnFuelPickedUp += Rocket_OnFuelPickedUp;
    }

    private void Rocket_OnFuelPickedUp(object sender, Rocket.OnFuelPickedUpEventArgs e)
    {
        ScorePopup scorePopup = Instantiate(scorePopupPrefab, e.fuelPickup.transform.position, Quaternion.identity);
        scorePopup.SetText("REFILL");
        Transform pickupCollectTransform = Instantiate(pickupCollectVfx, e.fuelPickup.transform.position, Quaternion.identity);
        Destroy(pickupCollectTransform.gameObject, 1f);

    }

    private void Rocket_OnCoinPickedUp(object sender, Rocket.OnCoinPickedUpEventArgs e)
    {
        ScorePopup scorePopup = Instantiate(scorePopupPrefab, e.coinPickup.transform.position, Quaternion.identity);
        scorePopup.SetText($"+{GameManager.COIN_PICKUP_SCORE}");
        Transform pickupCollectTransform = Instantiate(pickupCollectVfx, e.coinPickup.transform.position, Quaternion.identity);
        Destroy(pickupCollectTransform.gameObject, 1f);
    }

    private void Rocket_OnLanded(object sender, Rocket.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Rocket.LandingType.TooSteepAngle:
            case Rocket.LandingType.TooFastLanding:
            case Rocket.LandingType.WrongLandingArea:
                impulseSource.GenerateImpulse();
                break;
        }
    }
}
