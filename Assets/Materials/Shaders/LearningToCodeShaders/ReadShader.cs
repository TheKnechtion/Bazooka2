using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadShader : MonoBehaviour
{
    [SerializeField] Material mat;

    public Vector4 vec;


    // Update is called once per frame
    void Update()
    {

        vec = mat.GetVector("_uvVector");

    }
}
