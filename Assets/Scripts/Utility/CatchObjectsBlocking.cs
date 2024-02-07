using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine;
using System;

public class CatchObjectsBlocking : MonoBehaviour
{
    private Vector3 targetPos;
    private float distance;
    private Vector3 direction;

    private RaycastHit[] wallsHit = new RaycastHit[5];

    private int HitCOunt;

    private List<FadeObject> wallsFaded = new List<FadeObject>();

    [SerializeField] private LayerMask layerMask;

    private FadeObject fadeObj;

    private PlayerInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = PlayerInfo.instance;

        distance = Vector3.Distance(targetPos, transform.position);

        int maskToDetect = LayerMask.NameToLayer(layerMask.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        wallsFaded.Clear();
        targetPos = playerInfo.playerPosition;
        targetPos.y = 1;

        //direction = targetPos - gameObject.transform.position;
        direction = gameObject.transform.position - targetPos; //new


            //Ray centerRay = new Ray(gameObject.transform.position, direction);
        Ray centerRay = new Ray(targetPos, direction);//New
            //Debug.DrawRay(gameObject.transform.position, direction, Color.red);
        Debug.DrawRay(targetPos, direction, Color.red);//new

        //wallsHit = Physics.RaycastAll(gameObject.transform.position, direction, distance, layerMask);
        int hits = Physics.RaycastNonAlloc(centerRay, wallsHit, distance, layerMask);

        for (int i = 0; i < hits; i++)
        {
            if (wallsHit[i].transform.TryGetComponent<FadeObject>(out fadeObj))
            {
                wallsFaded.Add(fadeObj);
            }
            //FadeObject m = wallsHit[i].transform.GetComponent<FadeObject>();
            //wallsFaded.Add(m);
        }

        HitCOunt = hits - 1;
        Debug.Log("FadeObj hit: "+HitCOunt);

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
