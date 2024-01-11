using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Magnet : MonoBehaviour
{
    public GameObject magneticObject;
    [SerializeField] float speed;
    float initialPositionY;

    IEnumerator activeCoroutine;

    public void Move()
    {
        initialPositionY = this.transform.position.y;
        magneticObject = null;

        activeCoroutine = GoDown();

        StartCoroutine(activeCoroutine);   

    }

    private IEnumerator GoDown()
    {
        while (transform.position.y <= initialPositionY)
        {
            transform.position += (speed * Vector3.down);
            yield return null;
        }


    }

    private IEnumerator GoUp()
    {
        while(transform.position.y <= initialPositionY)
        {
            transform.position += (speed * Vector3.up);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "MagneticObject")
        {
            magneticObject = collision.gameObject;
            collision.transform.SetParent(this.transform);

            StopCoroutine(activeCoroutine);

            StartCoroutine(GoUp());            
        }
    }
}
