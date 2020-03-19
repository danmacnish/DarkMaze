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
    public PlayableDirector cameraTimeline;

    private Collider playerCollider;

    Pix.PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        if (playerCollider == null)
        {
            playerCollider = player.GetComponent<Collider>();
        }
        if (playerController == null)
        {
            playerController = player.GetComponent<Pix.PlayerController>();
        }
        if (gapController == null)
        {
            gapController = gap.GetComponent<GapController>();
        }
        if (targetWayPoint == null)
        {
            setWaypoint();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        walk();
    }

    void walk()
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
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
            cameraTimeline.Play();
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
            }
        }
    }
}
