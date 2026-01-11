using System;
using UnityEngine;

public class OldChainCargo : MonoBehaviour
{
    public event EventHandler OnAttachmentLost;

    [SerializeField] private OldCargoCrate cargoCrate;

    private CargoSO cargoSO;

    private bool isCargoAttached;
    public OldCargoCrate CargoCrate => cargoCrate;

    public bool IsCargoAttached => isCargoAttached;

    public CargoSO CargoSO { get => cargoSO; set => cargoSO = value; }

    private void Start()
    {
       // cargoCrate.OnCrashed += CargoCrate_OnCrashed;
       // cargoCrate.OnAnyPickedUp += CargoCrate_OnAnyPickedUp;
    }


    //private void CargoCrate_OnAnyPickedUp(object sender, CargoCrate.OnAnyPickedUpEventArgs e)
    //{
    //    e.pickup.OnCollected(Rocket.Instance);
    //}

    //private void CargoCrate_OnCrashed(object sender, CargoCrate.OnCargoCrashedEventArgs e)
    //{
    //    OnAttachmentLost?.Invoke(this, EventArgs.Empty);
    //}

    internal void DestroySelf()
    {
        Destroy(gameObject);
    }
}
