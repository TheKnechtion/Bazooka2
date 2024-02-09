using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{

    public bool isActive = false;
    public bool disappearOnDeactivation = false;
    public bool explodeWhenDeactivated = false;


    Explosive explosive = null;
    private void Start()
    {
        this.TryGetComponent<Explosive>(out explosive);
    }

    public virtual void Activate()
    {
        isActive = true;
    }

    public virtual void Deactivate()
    {
        isActive = false;

        if(explodeWhenDeactivated)
        {
            explosive.Explode();
            Destroy(this.gameObject);
        }

        if(disappearOnDeactivation)
        {
            Destroy(this.gameObject);
        }

    }
}
