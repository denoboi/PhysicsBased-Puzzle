using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private bool _isAlreadyScored;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAlreadyScored) //this prevents to score again with the same score area.
        {
            print("scored");
            _isAlreadyScored = true;
            
            GameManager.Instance.AddScore();
            
        }
    }
}