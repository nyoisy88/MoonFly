using JetBrains.Annotations;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Transform rocketStartTransform;
    [SerializeField] private Transform cameraStartTargetTransform;
    [SerializeField] private int zoomOutOrthographicSize;

    public int Level => level;
    public Vector3 RocketStartPosition => rocketStartTransform.position;
    public Transform CameraStartTargetTransfrom => cameraStartTargetTransform;

    public int ZoomOutOrthographicSize => zoomOutOrthographicSize;
}
