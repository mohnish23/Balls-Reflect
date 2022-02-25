using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierParameters : MonoBehaviour
{
    public GameObject[] RightBalls;
    public GameObject[] LeftBalls;
    public int hitCount;
    public int Quantity;

    private void Start()
    {
        if(Quantity > 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Quantity + "x";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(this.tag == "MultiplierWall" && collision.gameObject.CompareTag("DestructiveBall") || this.tag == "MultiplierWall" && collision.gameObject.CompareTag("Player"))
        {
            /*if either destructive ball or player ball hits multiplier wall, [DONE]
             * get a count of total active balls and add 1 to hit count everytime. 
             * If the hit count matches the total number of balls, start spawning.*/
            GameObject[] ActiveBalls = GameObject.FindGameObjectsWithTag("DestructiveBall");
            hitCount += 1;
        }
    }
}
