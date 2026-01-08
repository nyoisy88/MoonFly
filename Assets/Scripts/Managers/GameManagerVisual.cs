using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : Singleton<GameManagerVisual>
{   
    [SerializeField] protected CinemachineImpulseSource impulseSource;

    private void Start()
    {
        Rocket.Instance.OnLanded += Rocket_OnLanded;
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
