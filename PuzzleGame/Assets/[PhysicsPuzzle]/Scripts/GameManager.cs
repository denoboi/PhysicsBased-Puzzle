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
    
    [Header("Ball")]
    public GameObject Ball;
    private Rigidbody2D _ballRb;
    private Vector2 _ballStartPos;
    private float _ballDistanceToCam;
    private float _lastYPos; //Moving ball stuff

    public TextMeshProUGUI ScoreText, LivesText;
    
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _ballStartPos = Ball.transform.position;
       
        _lastYPos = _ballStartPos.y;
        _ballRb = Ball.GetComponent<Rigidbody2D>();
        UpdateTextElements();
    }

    public void GameOver()
    {
        //resetting ball position
        _ballStartPos.y = Camera.main.transform.position.y + _ballStartPos.y; //kamera hep asagi hareket edecegi icin gerek olacak.
        Ball.transform.position = _ballStartPos; //y pozisyonunu overwrite ettik 
        _ballRb.velocity = Vector2.zero; //In order to prevent fall speed when the game starts.
        
        _lives--;
        if (_lives <= 0)
        {
            _ballRb.isKinematic = true;
            print("GameOver");
        }
        
        UpdateTextElements();
    }

    public void AddScore()
    {
        _score++;
        
        UpdateTextElements();
    }

    public void UpdateTextElements()
    {
        ScoreText.text = "Score: " + _score.ToString("D4"); /*D4 makes it four decimal numbers.
                                                                   No matter how big or small the number is */
        LivesText.text = "Lives: " + _lives;
    }

    private void LateUpdate() //update'den daha gec ve daha az cagriliyor.
    {
        MoveCamWithTheBall();
    }

    void MoveCamWithTheBall()
    {
        if (Ball.transform.position.y <= Camera.main.transform.position.y)
        {
            Vector3 oldCamPos = Camera.main.transform.position;
            Vector3 newCamPos = new Vector3(oldCamPos.x, oldCamPos.y - 1f, oldCamPos.z);

            Camera.main.transform.position = Vector3.Lerp(oldCamPos, newCamPos, 2 * Time.deltaTime);
        }
    }
}
