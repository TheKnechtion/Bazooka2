using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource audioSource;



    public void StopEngineSound()
    {
        Destroy(this.gameObject.GetComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
