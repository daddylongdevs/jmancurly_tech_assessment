using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Score related components
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private ScoreUIHandler scoreUIHandler;

    // Player related components
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private PlayerCharacterController playerCharacterController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Camera mainCam;

    private void Awake()
    {
        // Initialize the camera controller first since the playerController relies on getting the main camera
        cameraController.Initialize(mainCam);
        playerInputHandler.Initialize(cameraController, playerCharacterController);
        scoreUIHandler.Initialize(scoreTracker);
        playerCharacterController.Initialize(cameraController);

        LockAndHideCursor();
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
