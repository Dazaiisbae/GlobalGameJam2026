using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        scoreText.text = "YOUR SCORE: " + lastScore;
        bestScoreText.text = "BEST SCORE: " + bestScore;
    }

    public void UpdateText()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "BEST SCORE: " + bestScore;
    }
}