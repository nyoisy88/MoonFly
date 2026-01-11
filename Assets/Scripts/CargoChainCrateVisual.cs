using System;
using UnityEngine;

public class CargoChainCrateVisual : MonoBehaviour
{
    [SerializeField] private GameObject cargoCrashVfx;

    private CargoChainCrate cargo;

    private void Start()
    {
        cargo = GetComponent<CargoChainCrate>();
        cargo.OnCargoCrashed += Cargo_OnCargoCrashed;
    }

    private void Cargo_OnCargoCrashed(object sender, Event.OnCargoCrashedEventArgs e)
    {
        GameObject cargoCrashVfxGO = Instantiate(cargoCrashVfx, (Vector3)e.crashPoint, Quaternion.identity);
        Destroy(cargoCrashVfxGO, 1f);
    }
}
