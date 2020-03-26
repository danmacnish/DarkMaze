using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{

    public List<PlayableAsset> sparkHitPlayables;
    public PlayableDirector Timeline;
    private int hitCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        hitCount = 0;
        SetPlayableAsset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit()
    {
        hitCount++;
        SetPlayableAsset();
    }

    private void SetPlayableAsset()
    {
        Timeline.playableAsset = sparkHitPlayables[hitCount];
    }
}
