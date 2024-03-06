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


        if(explodeWhenDeactivated)
        {
            explosive = this.transform.GetComponent<Explosive>();
        }


        if (explodeWhenDeactivated && disappearOnDeactivation)
        {
            explosive.CanDestroy += OnFinishedExploding;
        }

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
            return;
        }

        if(disappearOnDeactivation)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnFinishedExploding(object sender, System.EventArgs e)
    {
        explosive.CanDestroy -= OnFinishedExploding;
        Destroy(gameObject);
    }


}
