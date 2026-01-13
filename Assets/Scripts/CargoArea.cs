using System;
using UnityEngine;

public class CargoArea : MonoBehaviour
{
    public event EventHandler OnCargoDelivered;

    [Serializable]
    public enum InteractType
    {
        PickUp,
        DropOff,
    }

    [SerializeField] private InteractType interactType;
    [SerializeField] private float interactTimer = 2f;
    [SerializeField] private CargoSO cargoSO;

    private float timer;
    private Rocket rocket;

    public CargoSO CargoSO => cargoSO;
    public bool IsDisabled { get; private set; }

    private void Update()
    {
        if (IsDisabled) return;
        if (CanInteract())
        {
            timer += Time.deltaTime;
            if (timer > interactTimer)
            {
                CompleteInteraction();
                timer = 0f;
            }
        }
        else
        {
            timer -= Time.deltaTime / 2f;
        }

        timer = Mathf.Clamp(timer, 0f, interactTimer);
    }

    bool CanInteract()
    {
        if (interactType == InteractType.PickUp)
        {
            return rocket && !rocket.HasCargo();
        }
        else
        {
            return rocket && rocket.HasCargo() && rocket.CargoSO == this.cargoSO;
        }
    }

    void CompleteInteraction()
    {
        if (interactType == InteractType.PickUp)
        {
            rocket.PickUpCargo(cargoSO);
        }
        else
        {
            rocket.DeliverCargo();
            IsDisabled = true;
            OnCargoDelivered?.Invoke(this, EventArgs.Empty);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rocket rocket))
        {
            this.rocket = rocket;
            Debug.Log("Rocket enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rocket rocket))
        {
            this.rocket = null;
            Debug.Log("Rocket exit");
        }
    }

    public float GetProgressNormalized()
    {
        return timer / interactTimer;
    }
}
