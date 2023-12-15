using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip clipToPlay;
    [SerializeField] AudioClip clipToPlayOnDestroy;
    AudioSource audioSource;

    float loopClipTime;

    // Update is called once per frame
    void Update()
    {
        if(loopClipTime < Time.time)
        {
            AudioSource.PlayClipAtPoint(clipToPlay, this.gameObject.transform.position, 2.0f);
            loopClipTime = Time.time + clipToPlay.length;
        }
    }

    public void StopEngineSound()
    {
        Destroy(this);
    }


}
