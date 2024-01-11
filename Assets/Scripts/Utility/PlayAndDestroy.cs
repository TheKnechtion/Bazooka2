using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndDestroy : MonoBehaviour
{
    private ParticleSystem sysetem;

    // Start is called before the first frame update
    void Start()
    {
        sysetem= this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticles()
    {
        sysetem.Play();
        Destroy(gameObject, 3f);

    }
}
