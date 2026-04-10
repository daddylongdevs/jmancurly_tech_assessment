using UnityEngine;

public class BallDetector : MonoBehaviour
{
    public bool HasDetectedBall
    {
        get => ball != null;
    }

    private BallController ball;

    public BallController PickupBall()
    {
        BallController ballToPickup = ball;

        ball = null;

        return ballToPickup;
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController tryBall = other.GetComponent<BallController>();

        if (tryBall != null)
        {
            Debug.Log("I have found the ball.");
            ball = tryBall;
            // Trigger highlight on ball?
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BallController tryBall = other.GetComponent<BallController>();

        if(ball != null && ball == tryBall)
        {
            Debug.Log("The ball is gone.");
            ball = null;

            // Remove highlight on ball?
        }
    }
}
