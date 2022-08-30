using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public GameObject DotPrefab;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
            //where do we want to put our dot
            Vector2 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(DotPrefab, objPos, Quaternion.identity);
        }
      
    }
}
