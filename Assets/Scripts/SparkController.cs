using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Pix;

public class SparkController : MonoBehaviour
{
    public float restTime = 1.0f;
    // put the points from unity interface
    public Transform[] wayPointList;

    public Animator bridge; 

    public GameObject sphere;

    public GameObject pointLight;

    public GameObject gap;

    GapController gapController;

    public int currentWayPoint = 0;
    Transform targetWayPoint;

    public int endWayPoint = 1;

    public float speed = 4f;

    public bool stopWhenWaypointReached = false;

    private bool moving = true;

    public GameObject player;
    public PlayableDirector cameraTimeline;

    private Collider playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (playerCollider == null)
        {
            playerCollider = player.GetComponent<Collider>();
        }
        if (gapController == null)
        {
            gapController = gap.GetComponent<GapController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (targetWayPoint == null)
        {
            this.resetToStartPosition();
        }
        if (currentWayPoint > endWayPoint)
        {
            this.resetToStartPosition();
            this.showSpark(false);
            moving = false;
            Invoke("startMoving", restTime);
        }
        walk();
    }

    void startMoving()
    {
        moving = true;
        this.showSpark(true);
    }

    void walk()
    {
        if (moving)
        {
            // rotate towards the target
            transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

            // move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

            if (transform.position == targetWayPoint.position)
            {
                if (!stopWhenWaypointReached)
                {
                    currentWayPoint++;
                    targetWayPoint = wayPointList[currentWayPoint];
                }
                else
                {
                    this.showSpark(false);
                }
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

    public void resetToStartPosition()
    {
        currentWayPoint = 0;
        this.setWaypoint();
        transform.position = targetWayPoint.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            Debug.Log("spark hit player!");
            cameraTimeline.Play();
            //is player isn't in gap, then stop spark at player
            if (!gapController.isTriggered)
            {
                moveTo(other.transform);
                stopWhenWaypointReached = true;
                Invoke("resetSpark", (float)cameraTimeline.duration + 0.5f);
            }
            else
            {
                //otherwise if player is in gap, set spark to travel all the way until the end
                //open bridge
                bridge.SetTrigger("bridgeDown");
                endWayPoint = wayPointList.Length - 1;
                Invoke("resetSpark", (float)cameraTimeline.duration + 0.5f);
            }
        }
    }

    void resetSpark()
    {
        resetToStartPosition();
        showSpark(true);
        stopWhenWaypointReached = false;
    }

    void showSpark(bool show)
    {
        sphere.SetActive(show);
        pointLight.SetActive(show);

    }
}
