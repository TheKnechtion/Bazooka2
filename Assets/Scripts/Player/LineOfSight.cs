using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask CheckingMask;
    [SerializeField] private LayerMask EnvironmentMask;

    private ISpottable spottableType;

    private Vector3 direction;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.zero;
        distance = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        direction = (other.transform.position - gameObject.transform.position).normalized;
        distance = Vector3.Distance(gameObject.transform.position, other.transform.position);

        if (!Physics.Raycast(gameObject.transform.position, direction*distance, EnvironmentMask))
        {
            if (other.TryGetComponent<ISpottable>(out ISpottable ss))
            {
                ss.Spot();
            }
        }
        
    }
}
