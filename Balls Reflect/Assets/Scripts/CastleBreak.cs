using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CastleBreak : MonoBehaviour
{
    public List<GameObject> Blocks = new List<GameObject>();
    public int totalLength;
    public int breakCount;
    public int hp;
    public float currentHp;
    public bool Right; //if false go left, else go right

    public Color32[] hpCol;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;

        for (int i = 1; i < transform.childCount; i++)
        {
            Blocks.Add(transform.GetChild(i).gameObject);
        }

        for (int j = 0; j < Blocks.Count; j++)
        {
            Blocks = Blocks.OrderByDescending(x => x.transform.position.y).ToList();
        }

        totalLength = Blocks.Count;
        breakCount = (int) Mathf.Ceil(totalLength / hp);
    }

    private void Awake()
    {
        currentHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        var hpTxt = transform.GetChild(0).GetComponentInChildren<Text>();
        hpTxt.text = currentHp.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DestructiveBall") || collision.gameObject.CompareTag("Player"))
        {
            if(breakCount >= Blocks.Count)
            {
                for(int i = 0; i < Blocks.Count; i++)
                {
                    //Blocks[i].GetComponent<Rigidbody>().AddForce(0, 700, 700);

                    if (Right == true)
                    {
                        Blocks[i].GetComponent<Rigidbody>().AddForce(300, 700, 700);
                        Right = false;
                    }
                    else
                    {
                        Blocks[i].GetComponent<Rigidbody>().AddForce(-300, 700, 700);
                        Right = true;
                    }

                    Blocks.RemoveAt(i);
                }
            }
            else
            {
                for (int i = 0; i < breakCount; i++)
                {
                    //Destroy(Blocks[i]);
                    if(Right == true)
                    {
                        Blocks[i].GetComponent<Rigidbody>().AddForce(300, 600, 600);
                        Right = false;
                    }
                    else
                    {
                        Blocks[i].GetComponent<Rigidbody>().AddForce(-300, 600, 600);
                        Right = true;
                    }
                    
                    Blocks.RemoveAt(i);
                }
            }

            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
            
            if(currentHp <= 0)
            {
                currentHp = 0;
            }
            else
            {
                currentHp -= 1;
            }

            var bgImg = transform.GetChild(0).GetComponentInChildren<Image>();

            float per = (currentHp / hp) * 100;

            if (per > 80)
            {
                bgImg.color = hpCol[0];
            }
            else if (per > 30 && per < 80)
            {
                bgImg.color = hpCol[1];
            }
            else if (per < 30)
            {
                bgImg.color = hpCol[2];
            }

            Ball b = GameObject.Find("PlayerBall").GetComponent<Ball>();
            StartCoroutine(b.GameOverCheck());
        }

        /*if (collision.gameObject.CompareTag("DestructiveBall"))
        {
            collision.gameObject.GetComponent<BallMultiplier>().speed = 0;
        }*/
    }
}
