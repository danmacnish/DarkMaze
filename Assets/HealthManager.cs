using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthManager : MonoBehaviour
{

    private static HealthManager _instance;

    public static HealthManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public GameObject[] healthDots;

    public float fadeTime = 0.5f;

    private int _hits = 0;

    private int hits
    {
        set
        {
            _hits = (int)Mathf.Clamp(value, 0, healthDots.Length);
        }
        get
        {
            return _hits;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
        foreach (GameObject img in healthDots)
        {
            img.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hit()
    {
        Debug.Log("health hit!");
        hits += 1;
        for (int i = 0; i < hits; i++)
        {
            healthDots[i].SetActive(false);
        }
    }
}
