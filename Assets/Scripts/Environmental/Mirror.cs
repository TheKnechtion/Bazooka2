using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    RenderTexture mirrorTexture;

    public Material mirrorMaterial;

    [SerializeField] Camera mirrorCamera;
    [SerializeField] MeshRenderer mirrorRenderer;


    // Start is called before the first frame update
    void Start()
    {
        mirrorTexture = new RenderTexture(256,256,0);

        mirrorMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        mirrorCamera.targetTexture = mirrorTexture;

        mirrorMaterial.SetTexture("_BaseMap", mirrorTexture);

        mirrorRenderer.material = mirrorMaterial;

        mirrorCamera.cameraType = CameraType.Game;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
