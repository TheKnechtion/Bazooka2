using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [Tooltip("This trigger will only activate the 1st time it is passed through")]
    [SerializeField] private bool ActivateOnce;

    [Header("Things to do when triggered")]
    public UnityEvent TriggerEntered;
    private bool Triggered;
    void Start()
    {
        Triggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (ActivateOnce)
        {
            if (!Triggered)
            {
                Triggered = true;
                TriggerEntered.Invoke();
                Debug.Log("Tiggered");
            }
        }
        else
        {
            TriggerEntered.Invoke();
            Debug.Log("Tiggered");
        }

    }
}
