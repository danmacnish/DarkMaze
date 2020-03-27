using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Animations;

public class HealthManager : MonoBehaviour
{

    private static HealthManager _instance;

    public static HealthManager Instance { get { return _instance; } }


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

    public Animator anim;

    public float fadeTime = 0.5f;

    private int hits = 0;


    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hit(float delay = 0.0f)
    {
        StartCoroutine(_hit(delay));
    }

    private IEnumerator _hit(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("health hit!");
        hits += 1;
        anim.SetTrigger("hit");
    }
}
