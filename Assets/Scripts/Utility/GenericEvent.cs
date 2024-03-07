using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] DisableOnStart;
    [SerializeField] private GameObject[] ReferenceObjects;

    [SerializeField] private UnityEvent ReActivate;

    // Start is called before the first frame update
    void Start()
    {
        if (DisableOnStart.Length > 0)
        {
            for (int i = 0; i < DisableOnStart.Length; i++)
            {
                DisableOnStart[i].SetActive(false);
            }
        }
    }
}
