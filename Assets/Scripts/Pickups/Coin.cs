using UnityEngine;

public class Coin : MonoBehaviour, IPickup
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollected(Rocket rocket)
    {
        rocket.AddCoin(this);
        DestroySelf();
    }
}
