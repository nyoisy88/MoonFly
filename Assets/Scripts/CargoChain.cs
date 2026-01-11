using UnityEngine;

public class CargoChain : MonoBehaviour
{
    [SerializeField] private Transform cargoHoldPoint;

    public Transform CargoHoldPoint => cargoHoldPoint;

    public void Attach(CargoChainCrate cargo)
    {
        cargo.transform.SetParent(cargoHoldPoint, false);
    }
}
