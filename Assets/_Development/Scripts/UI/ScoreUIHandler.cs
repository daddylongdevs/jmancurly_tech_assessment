using System;
using TMPro;
using UnityEngine;

public class ScoreUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextObj;

    private ScoreTracker scoreTracker;

    public void Initialize(ScoreTracker scoreTracker)
    {
        this.scoreTracker = scoreTracker;

        if(scoreTracker == null)
        {
            Debug.LogError("Missing Score Tracker Object Reference.");
            return;
        }

        this.scoreTracker.onScoreUpdated += OnScoreUpdated;
    }

    private void OnScoreUpdated(int newScore)
    {
        if (scoreTextObj == null)
        {
            Debug.LogError("Missing Score Text Object.");
            return;
        }

        string scoreString = newScore.ToString();

        // TODO: Add some animations to the text on score increase
        scoreTextObj.text = scoreString;
    }

    private void OnDestroy()
    {
        if (scoreTracker == null)
        {
            Debug.LogError("Missing Score Tracker Object.");
            return;
        }

        scoreTracker.onScoreUpdated -= OnScoreUpdated;
    }
}
