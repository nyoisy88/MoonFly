using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator animator;

    private float autoFireDistance = 12f;
    private float autoFireTimer = 1f;
    private float timer;

    private void Update()
    {
        if (Rocket.Instance.CurrentState != Rocket.State.Active) return;
        Vector3 rocketDelta = Rocket.Instance.transform.position - container.position;
        if (rocketDelta.sqrMagnitude > autoFireDistance * autoFireDistance) 
            return;
        container.up = Vector3.Lerp(container.up, rocketDelta.normalized, 0.2f);
        timer += Time.deltaTime;
        if (timer > autoFireTimer)
        {
            timer = 0f;
            animator.SetTrigger("Shoot");
            GameObject bulletGO = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bulletGO.GetComponent<CannonBullet>().Init(container.up);
        }
    }
}
