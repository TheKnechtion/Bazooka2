using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera; //Players virtual cam will always be main camera

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        transform.forward = transform.position - mainCamera.transform.position;
    }
}
