using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Destroy(gameObject, 2.0f);
    }
}
