using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    Camera cam;
    GameObject playerObject;
    Vector3 playerPosition;


    Vector3 lockedRotation = new Vector3(55, 0, 0);



    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        playerObject = GameObject.Find("Player");
    }

    

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(lockedRotation);
    }
}
