using System;
using UnityEngine;

public class CargoChainCrate : MonoBehaviour
{
    public event EventHandler<Event.OnCargoCrashedEventArgs> OnCargoCrashed;

    [SerializeField] private CargoSO cargoSO;

    public CargoSO CargoSO => cargoSO;

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
            OnCargoCrashed?.Invoke(this, new Event.OnCargoCrashedEventArgs
            {
                crashPoint = collision.GetContact(0).point
            });
        }
    }
}

namespace Event
{
    public class OnCargoCrashedEventArgs : EventArgs
    {
        public Vector2 crashPoint;
    }
}