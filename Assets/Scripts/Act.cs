using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour, IActivate
{
    [SerializeField] float speed;
    [SerializeField] float targetHeight;

    public void Activate()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            
        StartCoroutine(MoveObjectForDuration(speed, targetPosition));
    }

    private IEnumerator MoveObjectForDuration(float speed, Vector3 targetPosition)
    {
        //while(elapsedTime < duration)
        while (transform.position.y > targetHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

    }


}
