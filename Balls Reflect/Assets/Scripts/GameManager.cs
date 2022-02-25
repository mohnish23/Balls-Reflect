using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool StartScreen = true;
    public GameObject TapToStart;

    public GameObject GameOverDisplay;
    public GameObject LevelCompleteDisplay;

    public GameObject[] Levels;

    // Start is called before the first frame update
    void Start()
    {
        Levels[PlayerPrefs.GetInt("LevelNum")].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(StartScreen == true)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                TapToStart.SetActive(false);
                StartScreen = false;
            }
        }

        if(GameObject.Find("BuildingBlocks").GetComponent<CastleBreak>().currentHp <= 0)
        {
            LevelCompleteDisplay.SetActive(true);
        }
    }
}
