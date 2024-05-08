using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pointsController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreBestText;
    private int score = 0;
    private int oldScore;
    private string scoreKey = "PlayerScore";

    public int getScore()
    {
        return score;
    }
    public void increasePoints(int points)
    {
        score += points;
        updateText();
        SaveScore();
    }

    void updateText()
    {
        if (scoreText != null && scoreBestText != null)
        {
            scoreText.text = score.ToString();
            scoreBestText.text = score.ToString();
        }
    }

    void SaveScore()
    {
        if(score > oldScore)
        {
            PlayerPrefs.SetInt(scoreKey, score);
            PlayerPrefs.Save();
        }
    }

    void LoadScore()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            oldScore = PlayerPrefs.GetInt(scoreKey);
        }
    }

    private void Start()
    {
        LoadScore();
        updateText();
    }

}
