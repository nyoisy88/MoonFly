using UnityEngine;

public class CargoChain : MonoBehaviour
{
    [SerializeField] private HingeJoint2D anchor;
    [SerializeField] private SpriteRenderer cargoSprite;

    public void Attach(Rigidbody2D rocketRb, CargoSO cargoSO)
    {
        anchor.connectedBody = rocketRb;
        cargoSprite.sprite = cargoSO.sprite;
    }
}
