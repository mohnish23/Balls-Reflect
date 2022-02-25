using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public bool move = true;
    public bool canShoot = true;
    public float speed;
    public LineRenderer AimLine;
    public Transform AimPoint;

    [SerializeField]
    private float touchSpeed = 0.023f;
    public Vector3 lastVelocity;
    public bool armed;

    public Vector3 PointOfCollision;
    public GameObject MultiplierBall;
    public int MultiplierVal;
    public int MultiplierLoop;

    public bool canMultiply = true;

    // Start is called before the first frame update
    void Start()
    {
        AimPoint = GameObject.Find("AimPoint").GetComponent<Transform>();
        canMultiply = true;
        rb = GetComponent<Rigidbody>();
        speed = 0;
    }

    private void FixedUpdate()
    {
        if(move == true)
        {
            transform.Translate(new Vector3(0, 0, speed), Space.Self);
        }

        if(canShoot == true)
        {
            //aim and shoot mechanism
            if (Input.touchCount > 0)
            {
                armed = true;
                Touch touch = Input.GetTouch(0);
                AimPoint.position = new Vector3(AimPoint.position.x + touch.deltaPosition.x * touchSpeed, AimPoint.position.y, AimPoint.position.z);
                transform.LookAt(AimPoint.position);
            }
            else
            {
                if (armed == true)
                {
                    //rb.AddForce(transform.forward * 1000f);
                    speed = 0.18f;
                    GetComponent<ShootLaser>().enabled = false;
                    Invoke("DestroyLine", 0.1f);
                    canShoot = false;
                    armed = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //bounds for aiming point
        AimPoint.position = new Vector3(Mathf.Clamp(AimPoint.position.x, -2.3f, 2.3f), AimPoint.position.y, AimPoint.position.z);
        transform.rotation = new Quaternion(transform.rotation.x, Mathf.Clamp(transform.rotation.y, -0.418659687f, 0.418659687f), transform.rotation.z, transform.rotation.w);

        //lock the Y position
        transform.position = new Vector3(transform.position.x, -13.93f, transform.position.z);

        //aim guidance system
        AimLine.SetPosition(0, transform.position);
        AimLine.SetPosition(1, AimPoint.position);

        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            /*var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * speed;*/

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if(collision.gameObject.CompareTag("Multiplier"))
        {
            var m = collision.gameObject.GetComponent<MultiplierParameters>();
            speed = 0;

            if (transform.eulerAngles.y > 5 && transform.eulerAngles.y < 180) //Ball is facing Right | hits mutiplier by Left side
            {
                m.RightBalls[0].SetActive(true);
                m.RightBalls[1].SetActive(true);
            }
            else if(transform.eulerAngles.y > 180 && transform.eulerAngles.y < 355) //Ball is facing Left | hits mutiplier by Right side
            {
                m.LeftBalls[0].SetActive(true);
                m.LeftBalls[1].SetActive(true);
            }
            else
            {
                m.RightBalls[0].SetActive(true);
                m.RightBalls[1].SetActive(true);
                m.LeftBalls[1].SetActive(true);
            }

            collision.gameObject.SetActive(false);
            Invoke("DestroyTrail", 1f);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            move = false;
        }

        if(collision.gameObject.CompareTag("MultiplierWall"))
        {
            PointOfCollision = transform.position;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);

            if (canMultiply == true)
            {
                MultiplierParameters m = collision.gameObject.GetComponent<MultiplierParameters>();
                MultiplierVal = m.Quantity;

                StartCoroutine(BallMultiplication());
                canMultiply = false;
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(GameOverCheck());
        }

        if (collision.gameObject.CompareTag("Hole"))
        {
            StartCoroutine(Shrink());
        }
    }

    void DestroyLine()
    {
        Destroy(GameObject.Find("Laser Beam"));
    }

    void DestroyTrail()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }

    public IEnumerator BallMultiplication()
    {
        for(int i = 0; i < MultiplierVal; i++)
        {
            yield return new WaitForSeconds(0.2f);

            if (i == MultiplierVal - 1)
            {
                GameObject x = Instantiate(MultiplierBall, PointOfCollision, transform.rotation);
                yield return new WaitForSeconds(0.1f);
                x.GetComponent<BallMultiplier>().canMultiply = true;
            }
            else
            {
                Instantiate(MultiplierBall, PointOfCollision, transform.rotation);
            }
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
        StartCoroutine(GameOverCheck());
    }

    public IEnumerator GameOverCheck()
    {
        yield return new WaitForSeconds(0.35f);

        GameObject[] ActiveBalls = GameObject.FindGameObjectsWithTag("DestructiveBall");
        var deadCount = 0;
        CastleBreak castle = GameObject.Find("BuildingBlocks").GetComponent<CastleBreak>();

        for (int i = 0; i < ActiveBalls.Length; i++)
        {
            if (ActiveBalls[i].GetComponent<MeshRenderer>().enabled == false)
            {
                deadCount += 1;
            }
        }

        if (castle.currentHp > 0 && deadCount >= ActiveBalls.Length && GetComponent<MeshRenderer>().enabled == false)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.GameOverDisplay.SetActive(true);
        }
    }
}
