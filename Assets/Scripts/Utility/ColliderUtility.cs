using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderUtility : MonoBehaviour
{
    public static void DeactivateColliders(Collider[] colliders)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public static void ActivateColliders(Collider[] colliders)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }



}

