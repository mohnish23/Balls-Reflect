using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextButton()
    {
        GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(PlayerPrefs.GetInt("LevelNum") < g.Levels.Length - 1)
        {
            PlayerPrefs.SetInt("LevelNum", PlayerPrefs.GetInt("LevelNum") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("LevelNum", 0);
        }
        PlayerPrefs.SetInt("LevelDisp", PlayerPrefs.GetInt("LevelDisp") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
