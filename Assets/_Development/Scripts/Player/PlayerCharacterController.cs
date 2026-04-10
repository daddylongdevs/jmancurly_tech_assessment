using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private BallDetector ballDetector;
    [SerializeField] private MeshRenderer bodyModel;
    [SerializeField] private Vector2 shootPowerRange = new Vector2(5f, 20f);
    [SerializeField] private float timeTilMaxShootPower = 1.0f;

    private CameraController cameraController;
    private Transform cameraTransform;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector2 moveInput;
    private bool isAiming = false;
    private BallController ball;
    private float shootPower = 5f;

    private Coroutine increaseThyShootPowerinator;

    public void Initialize(CameraController cameraController)
    {
        this.cameraController = cameraController;

        if (cameraController == null)
            return;

        cameraTransform = cameraController.Camera.transform;
    }

    private void Update()
    {
        Debug.Log(cameraTransform.forward);
        HandleMovement();
    }

    public void HandleInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    private void HandleMovement()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 forwardMovement = forward * moveInput.y;
        Vector3 rightMovement = right * moveInput.x;
        Vector3 moveDirection = (forwardMovement + rightMovement).normalized;

        if (moveDirection.sqrMagnitude >= 0.01f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void HandleActionInputPressed()
    {
        if(ball != null)
        {
            AimBall();
        }
        else
        {
            // Attempt to pick up ball
            TryAndPickupBall();
        }
    }

    public void HandleActionInputReleased()
    {
        TryAndExecuteShot();
    }

    public void HandleCancelActionInput()
    {
        if (ball != null)
        {
            if (isAiming)
            {
                isAiming = false;
                ball.CancelAim();

                if (increaseThyShootPowerinator != null)
                    StopCoroutine(increaseThyShootPowerinator);

                shootPower = shootPowerRange.x;
            }
            else
            {
                DropBall();
            }
        }
    }

    private void TryAndPickupBall()
    {
        if (ballDetector == null)
            return;

        if (ballDetector.HasDetectedBall)
        {
            // Multi ball support
            if(ball != null)
            {
                // Drop currently equipped ball
                DropBall();
            }

            ball = ballDetector.PickupBall();
            if (ball != null)
            {
                ball.Pickup();
                ballDetector.enabled = false;
            }
        }
    }

    private void DropBall()
    {
        if (ball == null)
            return;

        ball.Drop();
        ball = null;
        ballDetector.enabled = true;
    }

    private void AimBall()
    {
        if (ball == null)
            return;

        shootPower = shootPowerRange.x;

        bodyModel.enabled = false;
        ball.Aim();
        isAiming = true;
        cameraController.SetToFirstPersonCamera();

        if (increaseThyShootPowerinator != null)
            StopCoroutine(increaseThyShootPowerinator);
        increaseThyShootPowerinator = StartCoroutine(IncreaseShootPowerOnAim());
    }

    IEnumerator IncreaseShootPowerOnAim()
    {
        float elapsedTime = 0;

        while(elapsedTime < timeTilMaxShootPower)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / timeTilMaxShootPower;

            shootPower = Mathf.Lerp(shootPowerRange.x, shootPowerRange.y, t);
        }
    }

    private void TryAndExecuteShot()
    {
        if (ball == null || !isAiming)
            return;

        if (increaseThyShootPowerinator != null)
            StopCoroutine(increaseThyShootPowerinator);

        // If we have the ball and we're already aiming, shoot the ball
        ball.ExecuteShot(cameraTransform.forward, shootPower, -cameraTransform.right);
        cameraController.SetToThirdPersonCamera();
        
        bodyModel.enabled = true;
        isAiming = false;
        ball = null;
    }
}
