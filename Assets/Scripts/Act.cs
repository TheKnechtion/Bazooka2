using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour, IActivate
{
    
    public void Activate()
    {
        if (gameObject.name == "DoorOne")
        {
            Vector3 targetPosition = new Vector3(transform.position.x, -2f, transform.position.z);
            StartCoroutine(MoveObjectForDuration(10.0f, 1.0f, transform.position, targetPosition));
        }

    }

    private IEnumerator MoveObjectForDuration(float duration, float speed, Vector3 initialPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0.0f;
        
        while(elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPosition, speed*Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

    }


}
