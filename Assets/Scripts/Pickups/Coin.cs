using Signals;
using UnityEngine;

public class Coin : MonoBehaviour, IPickup
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollected(Rocket rocket)
    {
        SignalBus.Fire(new CoinPickedUpSignal
        {
            pickupPosition = transform.position,
        });
        DestroySelf();
    }
}
