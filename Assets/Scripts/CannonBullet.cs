using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private float flySpeed = 5.0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 direction)
    {
        rb.linearVelocity = flySpeed * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rocket rocket))
        {
            rocket.Destroyed();
        }
        Destroy(gameObject);
        
    }
}
