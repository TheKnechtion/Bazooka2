using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour, IActivate
{
    [SerializeField] float speed;
    [SerializeField] float targetHeight;

    float initialHeight;
    bool isMoving;
    Vector3 targetPosition;

    direction dir;
    
    
    enum direction
    {
        stopped,
        up,
        down
    }


    IEnumerator coroutineDown = null;
    IEnumerator coroutineUp = null;


    private void Awake()
    {
        dir = direction.stopped;
        initialHeight = transform.position.y;
        isMoving = false;

    }


    public virtual void Activate()
    {
        targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
        dir = direction.down;
        coroutineDown = MoveObject(speed, targetPosition);
        
        CouroutineCheck(coroutineUp);

        StartCoroutine(coroutineDown);
    }

    public void Deactivate()
    {
        targetPosition = new Vector3(transform.position.x, initialHeight, transform.position.z);
        dir = direction.up;
        coroutineUp = MoveObject(speed, targetPosition);

        CouroutineCheck(coroutineDown);
        
        StartCoroutine(coroutineUp);
    }
    //
    private IEnumerator MoveObject(float speed, Vector3 targetPosition)
    {
        //while(elapsedTime < duration)
        while (transform.position.y > targetHeight && dir == direction.down)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        while(transform.position.y < initialHeight && dir == direction.up)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    void CouroutineCheck(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }


}
