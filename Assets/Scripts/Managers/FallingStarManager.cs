using UnityEngine;

public class FallingStarManager : MonoBehaviour
{
    [SerializeField] private Transform fallingStarPrefab;
    [SerializeField] private Transform[] fallingStarSpawnPoints;

    private float spawnRate = 1f; // per second

    private void FixedUpdate()
    {
        if (Random.value < spawnRate * Time.fixedDeltaTime)
        {
            SpawnFallingStar();
        }

    }

    private void SpawnFallingStar()
    {

        Transform randomSpawnPoint = fallingStarSpawnPoints[Random.Range(0, fallingStarSpawnPoints.Length)];
        Transform fallingStarTransform = Instantiate(fallingStarPrefab, randomSpawnPoint.position, Quaternion.identity);
        Rigidbody2D fallingStarRb = fallingStarTransform.GetComponent<Rigidbody2D>();
        fallingStarRb.AddForce(new Vector2(Random.Range(-800, -300), 0f));
        Destroy(fallingStarTransform.gameObject, 5f);
    }
}
