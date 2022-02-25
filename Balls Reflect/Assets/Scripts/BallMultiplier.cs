using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMultiplier : MonoBehaviour
{
    public float speed = 0.18f;

    public Vector3 PointOfCollision;
    public GameObject MultiplierBall;
    public int MultiplierVal;
    public int MultiplierLoop;

    public bool canMultiply;
    public int activeBalls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, speed), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        //lock the Y position
        transform.position = new Vector3(transform.position.x, -13.93f, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (collision.gameObject.CompareTag("MultiplierWall"))
        {
            PointOfCollision = transform.position;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);

            if (canMultiply == true)
            {
                MultiplierParameters m = collision.gameObject.GetComponent<MultiplierParameters>();
                MultiplierVal = m.Quantity;

                activeBalls += m.Quantity;
                StartCoroutine(BallMultiplication());
                canMultiply = false;
            }
        }

        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Ball b = GameObject.Find("PlayerBall").GetComponent<Ball>();
            StartCoroutine(b.GameOverCheck());
        }

        if (collision.gameObject.CompareTag("Hole"))
        {
            StartCoroutine(Shrink());
        }
    }

    public IEnumerator BallMultiplication()
    {
        for (int i = 0; i < MultiplierVal; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Instantiate(MultiplierBall, PointOfCollision, transform.rotation);
        }
    }

    public IEnumerator Shrink()
    {
        yield return new WaitForSeconds(0.1f);
        speed = 0;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        Ball b = GameObject.Find("PlayerBall").GetComponent<Ball>();
        StartCoroutine(b.GameOverCheck());
    }
}
