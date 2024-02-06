using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask CheckingMask;

    private ISpottable spottableType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpottingVision()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SomethingEntered...");
        if (other.TryGetComponent<ISpottable>(out ISpottable ss))
        {
            Debug.Log("A spottable");
            ss.Spot();
        }
    }
}
