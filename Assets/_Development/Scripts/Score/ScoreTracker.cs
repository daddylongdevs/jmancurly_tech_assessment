using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public static int CURRENT_SCORE = 0;

    public System.Action<int> onScoreUpdated;

    [SerializeField] ScoreTriggerDetector detector;

    void Start()
    {
        if(detector != null)
        {
            detector.onValidScoreDetected += () => AddScore(1);
        }
        ResetScore();
    }

    public void AddScore(int amountToAdd)
    {
        CURRENT_SCORE += amountToAdd;

        onScoreUpdated?.Invoke(CURRENT_SCORE);
    }

    public void ResetScore()
    {
        CURRENT_SCORE = 0;

        onScoreUpdated?.Invoke(CURRENT_SCORE);
    }
}
