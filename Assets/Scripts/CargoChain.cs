using UnityEngine;

public class CargoChain : MonoBehaviour
{
    [SerializeField] private HingeJoint2D anchor;
    [SerializeField] private SpriteRenderer cargoSprite;
    [SerializeField] private Transform cargoHoldPoint;

    //public Transform CargoHoldPoint => cargoHoldPoint;

    public void Attach(Rigidbody2D rocketRb, CargoSO cargoSO)
    {
        anchor.connectedBody = rocketRb;
        cargoSprite.sprite = cargoSO.sprite;
    }
}
