using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            GameObject hinge = transform.parent.transform.gameObject;
            hinge.transform.rotation = Quaternion.Lerp(hinge.transform.rotation, Quaternion.Euler(new Vector3(0, 90, 0)), 1.0f);

            //hinge.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            Debug.Log(Quaternion.Euler(new Vector3(0, 90, 0)));

        }

    }


}
