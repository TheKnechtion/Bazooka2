using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float yValue;
    bool isElevating = false;
    float incrementVectorY = 0f;


    private void Awake()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.transform.tag == "Player") isElevating = true;

    }


    private void FixedUpdate()
    {
        if(isElevating)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, incrementVectorY, gameObject.transform.position.z);

            incrementVectorY += 0.05f;

            if(incrementVectorY >= yValue) isElevating=false;

        }

    }

}
