using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "New vignette settings")]
public class VignetteSO : ScriptableObject
{
    public float VignetteInstensity;
    public float VignettePower;
    public Color color;
    public float NoiseSpeed;
    public float NoisePower;
    public float NoiseIntensity;
}
