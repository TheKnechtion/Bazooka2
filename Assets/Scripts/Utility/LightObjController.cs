using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightObjController : MonoBehaviour
{
    /// <summary>
    /// Array of child light objects
    /// </summary>
    private Light[] heldLights;

    /// <summary>
    /// Array of all child lights starting colors, element wise matching
    /// </summary>
    private Color[] DefaultColors;

    /// <summary>
    /// When event is raised, changes all lights to this color
    /// </summary>
    [SerializeField] private Color ColorToChangeTo;

    [SerializeField] private bool RotateOnXAxis;
    [SerializeField] private float RotateSpeed;

    private Quaternion rotation;
    private void Awake()
    {
        int lightCount = gameObject.transform.childCount;
        heldLights = new Light[lightCount];
        DefaultColors = new Color[lightCount];

        rotation = new Quaternion();

        for (int i = 0; i < lightCount; i++)
        {
            heldLights[i] = gameObject.transform.GetChild(i).GetComponent<Light>();
            DefaultColors[i] = heldLights[i].color;
        }
    }

    private void Update()
    {
        if (RotateOnXAxis)
        {
            rotation.eulerAngles += new Vector3(0, RotateSpeed, 0);
            gameObject.transform.rotation = rotation;

            if (rotation.eulerAngles.y > 360)
                rotation.eulerAngles = Vector3.zero;
        }
    }

    public void ChangeLightColor()
    {
        for (int i = 0; i < heldLights.Length; i++)
        {
            heldLights[i].color = ColorToChangeTo;
        }
    }

    public void ResetColorValues()
    {
        for (int i = 0; i < heldLights.Length; i++)
        {
            heldLights[i].color = DefaultColors[i];
        }
    }

    
}
