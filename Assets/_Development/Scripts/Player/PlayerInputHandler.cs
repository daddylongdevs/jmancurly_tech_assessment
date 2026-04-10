using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference cancelAction;
    [SerializeField] private InputActionReference mouseAction;

    private CameraController cameraController;
    private PlayerCharacterController playerCharacterController;

    public void Initialize(CameraController cameraController, PlayerCharacterController playerCharacterController)
    {
        this.cameraController = cameraController;
        this.playerCharacterController = playerCharacterController;

        cameraController.SetTarget(playerCharacterController.transform);
    }

    private void OnEnable()
    {
        // Subscribe to input events
        shootAction.action.started += OnActionInputPressed;
        shootAction.action.canceled += OnActionInputReleased;
        cancelAction.action.started += OnCancelPressed;
        moveAction.action.performed += OnMoveInput;
        moveAction.action.canceled += OnMoveCancelled;
    }

    private void OnDisable()
    {
        // Unsubscribe to input events
        shootAction.action.started -= OnActionInputPressed;
        shootAction.action.canceled -= OnActionInputReleased;
        cancelAction.action.started -= OnCancelPressed;
        moveAction.action.performed -= OnMoveInput;
        moveAction.action.canceled -= OnMoveCancelled;
    }

    private void Update()
    {
        if (cameraController == null)
        {
            return;
        }


        Vector2 mouseDelta = mouseAction.action.ReadValue<Vector2>();
        cameraController.HandleLookInput(mouseDelta);
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        playerCharacterController.HandleInput(moveInput);
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        playerCharacterController.HandleInput(Vector2.zero);
    }

    private void OnActionInputPressed(InputAction.CallbackContext context)
    {
        playerCharacterController.HandleActionInputPressed();
    }

    private void OnActionInputReleased(InputAction.CallbackContext context)
    {
        playerCharacterController.HandleActionInputReleased();
    }

    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        playerCharacterController.HandleCancelActionInput();
    }
}