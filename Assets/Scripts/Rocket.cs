using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDir = GameInput.Instance.GetMovementInput();
    }

    private void FixedUpdate()
    {
        float force = 700f;
        if (moveDir.y > 0f)
        {
            rb.AddForce(force * Time.fixedDeltaTime * transform.up);
        }

        float turnSpeed = -100f;
        if (moveDir.x != 0f)
        {
            rb.AddTorque(moveDir.x * Time.fixedDeltaTime * turnSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed!");
            return;
        }
        float softLandingVelocityMagtitude = 5f;
        float landingSpeedMagtitude = collision.relativeVelocity.magnitude;
        if (landingSpeedMagtitude > softLandingVelocityMagtitude)
        {
            Debug.Log("Landing too hard");
            return;
        }

        float dotVectorLanding = Vector2.Dot(Vector2.up, transform.up);
        float minDotLandingVector = 0.9f;
        if (dotVectorLanding < minDotLandingVector)
        {
            Debug.Log("Landing Angle too steep!");
            return;
        }
        Debug.Log("Landing Success");

        float landingSpeedScoreMultiplier = 100f;
        float landingSpeedScore = (softLandingVelocityMagtitude - landingSpeedMagtitude) * landingSpeedScoreMultiplier;

        float maxLandingAngleScore = 100f ;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxLandingAngleScore - (1f - dotVectorLanding) * scoreDotVectorMultiplier * maxLandingAngleScore;
        Debug.Log($"Landing Angle Score: {landingAngleScore}");
        Debug.Log($"Landing Speed Score: {landingSpeedScore}");
    }
}
