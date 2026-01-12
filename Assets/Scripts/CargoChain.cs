using UnityEngine;

public class CargoChain : MonoBehaviour
{
    [SerializeField] private HingeJoint2D anchor;
    [SerializeField] private Transform cargoHoldPoint;

    public Transform CargoHoldPoint => cargoHoldPoint;

    public void Attach(Rigidbody2D rocketRb, CargoChainCrate cargo)
    {
        anchor.connectedBody = rocketRb;
        cargo.transform.SetParent(cargoHoldPoint, false);
    }
}
