using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] Activatable triggerThisObject;

    bool didOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !didOnce)
        {
            triggerThisObject.Activate();
            didOnce = true;
        }
    }

}
