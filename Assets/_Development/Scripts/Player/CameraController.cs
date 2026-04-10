using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera thirdPersonCameraPOV;
    [SerializeField] Camera firstPersonCameraPOV;

    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float tightSmoothTime = 0.05f;

    [Header("Look Settings")]
    [SerializeField] private Vector2 sensitivity = new Vector2(0.1f, 0.1f);
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private Transform targetTransform;
    private Camera targetCamera;
    private Vector3 currentVelocity;
    private Vector3 mainCamVelocity;
    private float yaw;
    private float pitch;

    private Camera mainCam;
    public Camera Camera
    {
        get => mainCam;
    }

    private float SmoothTimeToUse => targetCamera == firstPersonCameraPOV ? tightSmoothTime : smoothTime;

    public void Initialize(Camera mainCamera)
    {
        this.mainCam = mainCamera;

        if (mainCamera == null)
            return;

        CopyCameraSettingsImmediate(thirdPersonCameraPOV);
    }

    public void HandleLookInput(Vector2 mouseDelta)
    {
        yaw += mouseDelta.x * sensitivity.x;
        pitch -= mouseDelta.y * sensitivity.y;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    public void SetToFirstPersonCamera()
    {
        targetCamera = firstPersonCameraPOV;
    }

    public void SetToThirdPersonCamera()
    {
        targetCamera = thirdPersonCameraPOV;
    }

    public void CopyCameraSettingsImmediate(Camera sourceCamera)
    {
        if (sourceCamera == null)
            return;

        targetCamera = sourceCamera;
        mainCam.transform.position = sourceCamera.transform.position;
        mainCam.transform.rotation = sourceCamera.transform.rotation;

        mainCam.fieldOfView = sourceCamera.fieldOfView;
    }

    void LateUpdate()
    {
        if(mainCam == null || targetTransform == null) return;

        HandleRootCameraMovement();
        HandleTargetCameraMovement();
    }

    private void HandleTargetCameraMovement()
    {
        if(targetCamera == null) return;

        Vector3 targetPos = targetCamera.transform.position;

        Vector3 currentPos = mainCam.transform.position;

        mainCam.transform.position = Vector3.SmoothDamp(currentPos, targetPos, ref mainCamVelocity, SmoothTimeToUse);
        mainCam.transform.rotation = Quaternion.LookRotation(targetCamera.transform.forward, targetCamera.transform.up);

        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetCamera.fieldOfView, SmoothTimeToUse * Time.deltaTime);
    }

    private void HandleRootCameraMovement()
    {
        Vector3 targetPos = targetTransform.position;

        Vector3 currentPos = transform.position;

        Vector3 destination = new Vector3(targetPos.x, currentPos.y, targetPos.z);

        transform.position = Vector3.SmoothDamp(currentPos, destination, ref currentVelocity, SmoothTimeToUse);
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
