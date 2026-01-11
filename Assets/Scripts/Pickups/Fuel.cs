using UnityEngine;

public class Fuel : MonoBehaviour, IPickup
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollected(Rocket rocket)
    {
        rocket.AddFuel(this);
        DestroySelf();
    }
}
