using System;
using UnityEngine;

public class CargoArea : MonoBehaviour
{
    public event EventHandler OnInteractCompleted;

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

    private void Update()
    {
        if (CanInteract())
        {
            timer += Time.deltaTime;
            if (timer > interactTimer)
            {
                CompleteInteraction();
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
        return interactType == InteractType.PickUp
            ? rocket && !rocket.HasCargo()
            : rocket && rocket.HasCargo() && rocket.Cargo.CargoSO == this.cargoSO;
    }

    void CompleteInteraction()
    {
        if (interactType == InteractType.PickUp)
        {
            Transform cargoTransform = Instantiate(cargoSO.cargoPrefab);
            CargoChainCrate cargo = cargoTransform.GetComponent<CargoChainCrate>();
            rocket.PickUpCargo(cargo);
        }
        else
            rocket.DeliverCargo();

        OnInteractCompleted?.Invoke(this, EventArgs.Empty);
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
