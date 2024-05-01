using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "New vignette settings")]
public class VignetteSO : ScriptableObject
{
    [Range(0,2)]
    public float VignetteInstensity;

    [Range(0,10)]
    public float VignettePower;

    [ColorUsage(true, true)]
    public Color color;

    [Range(0,10)]
    public float NoiseSpeed;

    [Range(0,10)]
    public float NoisePower;

    [Range(0,2)]
    public float NoiseIntensity;

    public bool Transitions;
}
