using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int _score;
    private int _lives = 3;
    
    [Header("Ball")]
    public GameObject Ball;
    private Rigidbody2D _ballRb;
    private Vector2 _ballStartPos;
    private float _distanceToCam;
   

    public TextMeshProUGUI ScoreText, LivesText;
    
    //Spawn Traps
   
    private float _traveledDistance;
    private float _lastYPos; //Moving ball stuff
    
    public List<GameObject> TrapPrefabs = new List<GameObject>();
    public Transform SpawnPoint;

    private List<GameObject> _spawnedTraps = new List<GameObject>();


   
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _ballStartPos = Ball.transform.position;
        _distanceToCam = _ballStartPos.y;
        _lastYPos = _ballStartPos.y;
        _ballRb = Ball.GetComponent<Rigidbody2D>();
        UpdateTextElements(); 
        
        /*her 5 saniyede bir kamera uzerinde kalan trapleri kaldiriyoruz.
        string yapmamizin nedeni stop da yapacagiz. */
        StartCoroutine("DeleteTraps");  
    }

    public void GameOver()
    {
        //resetting ball position
        _ballStartPos.y = Camera.main.transform.position.y + _distanceToCam; //kamera hep asagi hareket edecegi icin gerek olacak.
        Ball.transform.position = _ballStartPos; //y pozisyonunu overwrite ettik, yani basa aldik tekrar topu.
        _ballRb.velocity = Vector2.zero; //In order to prevent fall speed when the game starts.
        
        _lives--;
        
        DeleteTrapsAboveCam(0);
        if (_lives <= 0)
        {
            _ballRb.isKinematic = true;
            print("GameOver");
            StopCoroutine("DeleteTraps"); //daha fazla metoda girmesini engelliyoruz.
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

     private void FixedUpdate()       //update'den daha gec ve daha az cagriliyor.
    {
        MoveCamWithTheBall();
        SpawnNewTraps();
    }

    void MoveCamWithTheBall()
    {
        if (Ball.transform.position.y <= Camera.main.transform.position.y)
        {
            Vector3 oldCamPos = Camera.main.transform.position;
            Vector3 newCamPos = new Vector3(oldCamPos.x, oldCamPos.y - 1f, oldCamPos.z);

            Camera.main.transform.position = Vector3.Lerp(oldCamPos, newCamPos, 3 * Time.fixedDeltaTime);
        }
    }

    void SpawnNewTraps()
    {
        float distanceToNewSpawn = Random.Range(4f, 12f); //globalde tanimlanmiyor random.range
        
        _traveledDistance = _lastYPos - Ball.transform.position.y; //baslangicta lastY position start'taki topun y degeri(float).

        if (_traveledDistance >= distanceToNewSpawn)
        {
            _lastYPos = Ball.transform.position.y; //simdi lasty pozisyonu degistiriyoruz.
            _traveledDistance = 0;
            InstantiateNewTraps();

            Debug.Log("New trap created");
        }
        
    }

    void InstantiateNewTraps()
    {
        int index = Random.Range(0, TrapPrefabs.Count);
        GameObject newTrap = Instantiate(TrapPrefabs[index], SpawnPoint.position, Quaternion.identity);
        _spawnedTraps.Add(newTrap);
    }

    void DeleteTrapsAboveCam(float distance)
    {
        for (int i = _spawnedTraps.Count - 1; i >= 0; i--)
        {
            if (_spawnedTraps[i].transform.position.y > Camera.main.transform.position.y + distance)
            {
                Destroy(_spawnedTraps[i]);
                _spawnedTraps.RemoveAt(i); //listeden de cikaracagiz. null olmamasi icin.
            }
        }
    }

    IEnumerator DeleteTraps()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DeleteTrapsAboveCam(5f);
        }
       
    }
}
