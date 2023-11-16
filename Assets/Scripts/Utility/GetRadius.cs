using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRadius : MonoBehaviour
{
    public float radius;
    private ParticleSystem particalSystem;

    // Start is called before the first frame update
    void Start()
    {
        particalSystem= GetComponent<ParticleSystem>();
        radius = particalSystem.shape.radius;

        Destroy(particalSystem, 2.0f);
    }
}
