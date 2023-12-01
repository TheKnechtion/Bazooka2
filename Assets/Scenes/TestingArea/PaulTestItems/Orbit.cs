using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject pointOne;
    public GameObject pointTwo;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0,pointOne.transform.position);
        lineRenderer.SetPosition(1, pointTwo.transform.position);
        
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
    }
}
