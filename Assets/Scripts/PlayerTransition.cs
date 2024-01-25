using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TransitionPlayerToStartPosition();
    }


    GameObject player;

    void TransitionPlayerToStartPosition()
    {
        player = GameObject.Find("Player");
        player.transform.position = gameObject.transform.position;



        Destroy(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
