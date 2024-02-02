using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnDemand : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleFX;

    private IParticleCaller Invoker;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponentInParent<IParticleCaller>() != null)
        {
            Invoker = gameObject.GetComponentInParent<IParticleCaller>();
        }
        else if (gameObject.GetComponent<IParticleCaller>() != null)
        {
            Invoker = gameObject.GetComponentInParent<IParticleCaller>();
        }
        else
        {
            Invoker = gameObject.GetComponentInChildren<IParticleCaller>();
        }

        Invoker.OnParticleCall += OnParticlesRequested;
    }

    private void OnParticlesRequested(object sender, System.EventArgs e)
    {
        if (ParticleFX)
        {
            ParticleFX.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
