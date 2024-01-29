using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSettings : MonoBehaviour
{
    public LineRenderer Renderer { get; set; }
    public Vector3 Position 
    {
        get { return gameObject.transform.position; } 
        set { gameObject.transform.position = value; } 
    }
    public float LaserWidth 
    { 
        get { return Renderer.widthMultiplier; }
        set { Renderer.widthMultiplier = value; }
    }

    private void Awake()
    {
        Renderer = GetComponent<LineRenderer>();

    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
