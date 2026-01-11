using System;
using UnityEngine;

public class OldCargoCrate : MonoBehaviour
{
    public class OnCargoCrashedEventArgs : EventArgs
    {
        public Vector2 crashPoint;
    }

    public class OnAnyPickedUpEventArgs : EventArgs
    {
        public IPickup pickup;
    }

    public event EventHandler<OnCargoCrashedEventArgs> OnCrashed;
    public event EventHandler<OnAnyPickedUpEventArgs> OnAnyPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPickup pickup))
        {
            OnAnyPickedUp?.Invoke(this, new OnAnyPickedUpEventArgs
            {
                pickup = pickup
            });
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad _))
        {
            OnCrashed?.Invoke(this, new OnCargoCrashedEventArgs
            {
                crashPoint = collision.GetContact(0).point,
            });
        }
    }
}
