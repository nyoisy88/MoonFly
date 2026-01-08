using Unity.Cinemachine;
using UnityEngine;

public class CameraOrthographicZoom2D : MonoBehaviour
{
    public const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    public static CameraOrthographicZoom2D Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize;

    public float TargetOrthographicSize { get => targetOrthographicSize; set => targetOrthographicSize = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, TargetOrthographicSize, zoomSpeed * Time.deltaTime);
    }

    public void SetNormalOrthographicSize()
    {
        targetOrthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
    }
}
