using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Pix;

public class SparkController : MonoBehaviour
{

    // put the points from unity interface
    public List<Transform> wayPointList;

    public Animator bridge;

    public GameObject sphere;

    public GameObject pointLight;

    public GameObject gap;

    GapController gapController;

    public int currentWayPoint = 0;
    Transform targetWayPoint;

    public int endWayPoint = 1;

    public float speed = 4f;

    public bool destroyWhenWaypointReached = false;

    public GameObject player;

    private Collider playerCollider;

    Pix.PlayerTouchController playerController;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if (playerCollider == null)
        {
            playerCollider = player.GetComponent<Collider>();
        }
        if (playerController == null)
        {
            playerController = player.GetComponent<Pix.PlayerTouchController>();
        }
        if (gapController == null)
        {
            gapController = gap.GetComponent<GapController>();
        }
        if (targetWayPoint == null)
        {
            setWaypoint();
        }
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        walk();
    }

    void walk()
    {
        // rotate towards the target
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);
        Vector3 direction = targetWayPoint.position - transform.position;
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime );

        // move towards the target
        //transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWayPoint.position) < 0.1)
        {
            if (!destroyWhenWaypointReached)
            {
                currentWayPoint++;
                targetWayPoint = wayPointList[currentWayPoint];
                if (currentWayPoint == endWayPoint)
                {
                    destroyWhenWaypointReached = true;
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void moveTo(Transform location)
    {
        targetWayPoint = location;
    }

    void setWaypoint()
    {
        targetWayPoint = wayPointList[currentWayPoint];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            HealthManager.Instance.hit();
            playerController.Hit();
            Debug.Log("spark hit player!");
            //is player isn't in gap, then stop spark at player
            if (!gapController.isTriggered)
            {
                moveTo(other.transform);
                destroyWhenWaypointReached = true;
            }
            else
            {
                //otherwise if player is in gap, set spark to travel all the way until the end
                //open bridge
                bridge.SetTrigger("bridgeDown");
                endWayPoint = wayPointList.Count - 1;
                destroyWhenWaypointReached = false;
            }
        }
    }
}
