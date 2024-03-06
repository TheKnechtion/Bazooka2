using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Sander_Movement : Activatable
{

    [SerializeField] float speed;

    Directions currentDirection;

    bool didOnce = false;

    public bool doesLoop;

    private void Awake()
    {
        currentDirection = Directions.Right;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && isActive)
        {
            other.transform.GetComponent<PlayerMovement>().Flattened();
            return;
        }

        if (other.transform.tag == "Break")
        {
            AudioManager.PlayClipAtPosition("explosion_sound", transform.position);
            Destroy(other.transform.gameObject);
            return;
        }


        if (other.transform.tag == "Collide" && doesLoop)
        {
            AudioManager.PlayClipAtPosition("hit_sound", transform.position);
            ChangeDirection();
        }
        else if(other.transform.tag == "Collide" && !didOnce)
        {
            AudioManager.PlayClipAtPosition("hit_sound", transform.position);
            didOnce = true;
            Deactivate();
            speed = 0;
        }
    }

    void ChangeDirection()
    {
        if(currentDirection == Directions.Right)
        {
            currentDirection = Directions.Left;
        }
        else 
        {
            currentDirection = Directions.Right;
        }
    }

    void FixedUpdate()
    {

        if(isActive && currentDirection == Directions.Right)
        {
            this.transform.position += (this.transform.right * speed);
        }

        if(isActive && currentDirection == Directions.Left)
        {
            this.transform.position -= (this.transform.right * speed);
        }

    }




}
