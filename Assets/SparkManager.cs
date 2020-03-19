using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SparkManager : MonoBehaviour
{
    public GameObject sparkPrefab;
    public float delayBetweenSparks = 1.0f;
    private static SparkManager _instance;

    public static SparkManager Instance { get { return _instance; } }

    public GameObject SparkWaypoints;
    private List<Transform> waypoints;

    public int endWayPoint = 1;

    public float speed = 4f;

    public GameObject player;
    public PlayableDirector cameraTimeline;

    public GameObject gap;

    public Animator bridge;

    float time = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<Transform>();
        foreach (Transform t in SparkWaypoints.transform)
        {
            Debug.Log(t);
            waypoints.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= delayBetweenSparks)
        {
            InstantiateSpark();
            time = 0;
        }
    }

    private void InstantiateSpark()
    {
        GameObject spark = Instantiate(sparkPrefab, waypoints[0].position, waypoints[0].rotation, transform);
        SparkController controller = spark.GetComponent<SparkController>();
        controller.wayPointList = waypoints;
        controller.player = player;
        controller.speed = speed;
        controller.cameraTimeline = cameraTimeline;
        controller.bridge = bridge;
        controller.gap = gap;
        controller.endWayPoint = endWayPoint;
    }
}
