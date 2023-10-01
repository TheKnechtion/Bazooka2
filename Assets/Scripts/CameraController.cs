using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    Camera cam;
    GameObject playerObject;
    Vector3 playerPosition;





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
    void Update()
    {
        playerPosition = playerObject.transform.position;

        this.gameObject.transform.position = new Vector3(playerPosition.x, 12, playerPosition.z - 4);
    }
}
