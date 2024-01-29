using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Tram : MonoBehaviour
{
    [SerializeField] GameObject[] targets;

    [SerializeField] bool loop;

    [SerializeField] float speed;

    float moveSpeed;

    Vector3 currentTarget;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targets[i].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            other.transform.SetParent(this.transform);

            moveSpeed = speed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(null);
            DontDestroyOnLoad(other.transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.position != currentTarget)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget, moveSpeed);
        }
        else if(i < targets.Length - 1)
        {
            i++;
            currentTarget = targets[i].transform.position;
        }
        else if(loop)
        {
            i = 0;
            currentTarget = targets[i].transform.position;
        }


    }
}
