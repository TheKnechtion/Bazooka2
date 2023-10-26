using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine;
using System;

public class CatchObjectsBlocking : MonoBehaviour
{
    private Vector3 targetPos;
    private PlayerInfo Info;

    private float distance;
    private Vector3 direction;
    private Vector3 directionA, directionB, directionC, directionD;

    private RaycastHit[] wallsHit = new RaycastHit[5];

    private List<FadeObject> wallsFaded = new List<FadeObject>();

    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        Info = PlayerInfo.instance;
        distance = Vector3.Distance(targetPos, transform.position);

        int maskToDetect = LayerMask.NameToLayer(layerMask.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        wallsFaded.Clear();

        targetPos = PlayerInfo.instance.playerPosition;

        direction = targetPos - gameObject.transform.position;
        Ray centerRay = new Ray(gameObject.transform.position, direction);
        Debug.DrawRay(gameObject.transform.position, direction, Color.red);

        //wallsHit = Physics.RaycastAll(gameObject.transform.position, direction, distance, layerMask);
        int hits = Physics.RaycastNonAlloc(centerRay, wallsHit, distance, layerMask);

        for (int i = 0; i < hits; i++)
        {
            FadeObject m = wallsHit[i].transform.GetComponent<FadeObject>();
            wallsFaded.Add(m);
        }

        fadeAndUnfade();

        
    }

    private void fadeAndUnfade()
    {
        foreach (FadeObject item in wallsFaded)
        {
            item.FadeThis();
        }
    }

}
