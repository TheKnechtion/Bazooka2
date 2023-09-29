using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    Camera cam;
    [SerializeField] GameObject playerObject;
    Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerObject.transform.position;

        cam.transform.position = new Vector3(playerPosition.x, 12, playerPosition.z - 4);
    }
}
