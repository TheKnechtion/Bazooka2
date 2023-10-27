using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivatable : MonoBehaviour, IActivate
{
    public void Activate()
    {
        transform.position = new Vector3(66, 66, 66);
    }

}
