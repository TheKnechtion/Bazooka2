using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] Activatable[] triggerTheseObjects;

    bool didOnce = false;

    private void Start()
    {
        Destroy(gameObject.GetComponent<MeshRenderer>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !didOnce)
        {
            foreach(Activatable triggerThisObject in triggerTheseObjects)
            {
                triggerThisObject.Activate();
            }

            didOnce = true;
        }
    }

}
