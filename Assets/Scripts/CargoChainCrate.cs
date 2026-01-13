using Signals;
using UnityEngine;

public class CargoChainCrate : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPickup pickup))
        {
            pickup.OnCollected(Rocket.Instance);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad _))
        {
            Rocket.Instance.CargoCrashed();
            SignalBus.Fire(new CargoCrashedSignal
            {
                crashPoint = collision.GetContact(0).point
            });
        }
    }
}