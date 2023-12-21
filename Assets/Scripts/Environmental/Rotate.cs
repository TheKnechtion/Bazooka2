using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    Quaternion targetQuat;
    float RotateY;
    float RotateZ;

    private void Awake()
    {
        RotateY = transform.localEulerAngles.y;
        //RotateZ = transform.localEulerAngles.z;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(-30f*Time.time, RotateY, 0);
    }
}
