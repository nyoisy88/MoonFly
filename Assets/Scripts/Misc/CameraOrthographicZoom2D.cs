using Unity.Cinemachine;
using UnityEngine;

public class CameraOrthographicZoom2D : Singleton<CameraOrthographicZoom2D>
{
    public const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize;

    public float TargetOrthographicSize { get => targetOrthographicSize; set => targetOrthographicSize = value; }


    void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize =
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, 
                        targetOrthographicSize, 
                        zoomSpeed * Time.deltaTime);
    }

    public void SetNormalOrthographicSize()
    {
        targetOrthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
    }
}
