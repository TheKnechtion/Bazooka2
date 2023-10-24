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
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        render.material.shader = transparentShader;
        yield return new WaitForSeconds(0.2f);
        render.material.shader = initialShader;
        yield return null;
    }
}
