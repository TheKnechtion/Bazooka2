using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private Renderer render;

    [SerializeField]private Shader transparentShader;
    private Shader initialShader;


    void Start()
    {
        render= GetComponent<Renderer>();
        initialShader = render.material.shader;
    }

    public void FadeThis()
    {
        render.material.shader = transparentShader;
    }

    public void UnfadeThis()
    {
        render.material.shader = initialShader;
    }
}
