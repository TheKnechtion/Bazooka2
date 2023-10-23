using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine;
using System;

public class CatchObjectsBlocking : MonoBehaviour, IEquatable<FadeObject>
{
    private Vector3 targetPos;
    private float distance;
    private Vector3 direction;

    private RaycastHit[] wallsHit = new RaycastHit[5];
    private List<FadeObject> wallsFaded = new List<FadeObject>();

    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(targetPos, transform.position);

        int maskToDetect = LayerMask.NameToLayer(layerMask.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = PlayerInfo.instance.playerPosition;

        direction = targetPos - gameObject.transform.position;
        Ray ray = new Ray(gameObject.transform.position, direction * distance);
        Debug.DrawRay(gameObject.transform.position, direction * distance, Color.red);

        wallsHit = Physics.RaycastAll(gameObject.transform.position, direction, distance, layerMask);

        fadeAndUnfade();
        
        
    }

    private void fadeAndUnfade()
    {
        for (int i = 0; i < wallsHit.Length; i++)
        {
            FadeObject indexHit = wallsHit[i].transform.GetComponent<FadeObject>();
            indexHit.FadeThis();
            wallsFaded.Add(indexHit);
        }

        foreach (FadeObject item in wallsFaded)
        {
            for (int i = 0; i < wallsHit.Length; i++)
            {
                FadeObject indexHit = wallsHit[i].transform.GetComponent<FadeObject>();
                if (indexHit != item)
                {
                    item.UnfadeThis();
                }
            }
        }
    }

    public bool Equals(FadeObject other)
    {
        throw new NotImplementedException();
    }
}
