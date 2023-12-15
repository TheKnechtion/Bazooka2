using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using TreeEditor;
//using UnityEditor.Callbacks;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private Renderer render;

    //[SerializeField] private Shader transparentShader;
    private Shader initialShader;

    [SerializeField] private bool UseDefaultFields;
    [SerializeField] private float Fade_To_Opacity;

    private float opacity;
    private float t;
    public float fadeSpeed;



    void Start()
    {
        if (UseDefaultFields)
        {
            Fade_To_Opacity = 0.1f;
            fadeSpeed = 3.0f;
        }
        render= GetComponent<Renderer>();
        initialShader = render.material.shader;
    }

    public void FadeThis()
    {
        StopAllCoroutines();
        StartCoroutine(fadeOverTime(1, Fade_To_Opacity));
    }

    private void Update()
    {

    }

    private IEnumerator fadeOverTime(float fromAlpha, float toAlpha)
    {
        //t = 0;
        //render.material.shader = transparentShader;

        while (opacity != toAlpha)
        {
            opacity = Mathf.Lerp(fromAlpha, toAlpha, t);
            render.material.SetFloat("_Opacity", opacity);

            t += fadeSpeed * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        t = 0;

        while (opacity != fromAlpha)
        {
            opacity = Mathf.Lerp(toAlpha, fromAlpha, t);
            render.material.SetFloat("_Opacity", opacity);

            t += fadeSpeed * Time.deltaTime;

            yield return null;
        }

        //render.material.shader = initialShader;
        t = 0;

        yield return null;
    }

    #region Old Fade Coroutine
    //private IEnumerator Fade()
    //{
    //    //opacity = 1.0f;

    //    //if (render.material != DitherMaterial)
    //    //{
    //    //    render.material = DitherMaterial;
    //    //}
    //    render.material.shader = transparentShader;


    //    //if (opacity >= 0.3f)
    //    //{
    //    //    opacity -= 0.05f;
    //    //    render.material.SetFloat("_Opacity", opacity);
    //    //}

    //    //while (opacity > 0.1f)
    //    //{
    //    //    //opacity -= 0.01f;

    //    //    render.material.SetFloat("_Opacity", opacity);
    //    //}
    //    render.material.SetFloat("_Opacity", opacity);

    //    yield return new WaitForSeconds(0.2f);
    //    render.material.shader = initialShader;
    //    //render.material.SetFloat("_Opacity", 1.0f);
    //    //render.material = initMaterial;

    //    yield return null;
    //}
    #endregion
}
