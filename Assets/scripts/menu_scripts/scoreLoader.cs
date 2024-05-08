using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scoreLoader : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private string scoreKey = "PlayerScore";
    void Start()
    {
        LoadScore();
    }
    void LoadScore()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            int score = PlayerPrefs.GetInt(scoreKey);
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            scoreText.text = "Score: Not Available";
        }
    }
}
