using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _score;
    private int _lives = 3;

    public TextMeshPro ScoreText, LivesText;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        _lives--;
        if (_lives == 0)
        {
            print("GameOver");
        }
    }

    public void AddScore()
    {
        _score++;
    }

    public void UpdateTextElements()
    {
        ScoreText.text = "Score: " + _score.ToString("D4"); /*D4 makes it four decimal numbers.
                                                                   No matter how big or small the number is */
        LivesText.text = "Lives: " + _lives;
    }

   
    
}
