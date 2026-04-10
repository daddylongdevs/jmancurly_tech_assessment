using System.Collections;
using UnityEngine;

public class ScoreTriggerDetector : MonoBehaviour
{
    [SerializeField] private float resetDelay = 1f;
    public System.Action onValidScoreDetected;

    private BallController ballRef;

    private void OnTriggerEnter(Collider other)
    {
        BallController ball = other.GetComponent<BallController>();

        if (other == null || ball == null)
            return;

        bool isBallDetectedIsUnderCollider = ball.transform.position.y < transform.position.y;

        if (isBallDetectedIsUnderCollider)
            return;

        ballRef = ball;
    }

    private void OnTriggerExit(Collider other)
    {
        if (ballRef == null)
            return;

        BallController ball = other.GetComponent<BallController>();

        if (other == null || ball == null || ball != ballRef)
            return;

        bool isBallDetectedIsOverCollider = ball.transform.position.y > transform.position.y;

        if(ball.transform.position.y > transform.position.y)
        {
            ResetState();
        }
        else
        {
            Debug.Log("Valid ");
            onValidScoreDetected?.Invoke();
        }
    }

    public void ResetState()
    {
        ballRef = null;
        StopAllCoroutines();
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        ResetAfterDelay();
    }
}
