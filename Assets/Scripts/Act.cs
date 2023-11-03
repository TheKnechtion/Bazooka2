using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour, IActivate
{
    public float timeToCompleteActivation = 10.0f;
    public float speed = 1.0f;

    public float targetHeight;
    public void Activate()
    {
        if (gameObject.name.Contains("Door") || gameObject.name.Contains("Door"))
        {   
            Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            
            StartCoroutine(MoveObjectForDuration(timeToCompleteActivation, speed, transform.position, targetPosition));
        }

    }

    private IEnumerator MoveObjectForDuration(float duration, float speed, Vector3 initialPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0.0f;
        
        while(elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

    }


}
