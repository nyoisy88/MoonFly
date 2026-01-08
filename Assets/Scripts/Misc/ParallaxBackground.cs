using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxMulitplier = .1f;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = Camera.main.transform.position;
    }
    private void LateUpdate()
    {
        Vector3 newCameraPosition = Camera.main.transform.position;
        Vector3 positionDelta = newCameraPosition - lastCameraPosition;
        transform.position +=  positionDelta * parallaxMulitplier;
        lastCameraPosition = newCameraPosition;
    }
}
