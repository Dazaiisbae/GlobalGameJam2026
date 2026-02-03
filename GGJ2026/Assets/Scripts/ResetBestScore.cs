using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBestScore : MonoBehaviour
{
    public BestScoreUI bestScoreUI; // drag this in Inspector

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("BestScore");
        PlayerPrefs.Save();

        Debug.Log("Best score reset");

        // Refresh UI immediately
        bestScoreUI.UpdateText();
    }
}
