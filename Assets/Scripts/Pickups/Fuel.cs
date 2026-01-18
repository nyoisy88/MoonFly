using Signals;
using UnityEngine;

public class Fuel : MonoBehaviour, IPickup
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollected(Rocket rocket)
    {
        SignalBus.Fire(new FuelPickedUpSignal
        {
            pickupPosition = this.transform.position
        });
        float fuelAmount = 12f;
        rocket.AddFuel(fuelAmount);
        DestroySelf();
    }
}
