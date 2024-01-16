using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftShaderValueWithTime : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] string shaderPropertyName;

    public bool change = false;

    float track = 0;

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            track += Time.deltaTime;
            mat.SetFloat(shaderPropertyName, track);
        }
    }
}
