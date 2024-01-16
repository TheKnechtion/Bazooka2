using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;


public class MirrorLightReflection : MonoBehaviour
{
    [SerializeField] LineRenderer lightBeams;

    public UnityEvent OnActivated;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    
    RaycastHit mirrorPoint;

    Vector3 reflectVector;

    int bounceCount = 0;

    bool hasActivated = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        StartLightBeam();

        BounceLightBeam();

    }

    void StartLightBeam()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit point))
        {
            lightBeams.positionCount = 0;
            lightBeams.positionCount++;
            lightBeams.SetPosition(lightBeams.positionCount-1, point.point);
            mirrorPoint = point;
            reflectVector = Vector3.Reflect(this.transform.forward, mirrorPoint.normal);
        }
    }

    RaycastHit tempPoint;
    void BounceLightBeam()
    {
        // 
        if (Physics.Raycast(mirrorPoint.point, reflectVector, out RaycastHit point) && mirrorPoint.collider.gameObject.tag == "Mirror" && lightBeams.positionCount < 5)
        {
            lightBeams.positionCount++;
            lightBeams.SetPosition(lightBeams.positionCount-1, point.point);
            mirrorPoint = point;
            reflectVector = Vector3.Reflect(reflectVector, mirrorPoint.normal);
            BounceLightBeam();

        }
        else
        {
            tempPoint = point;
        }

        if (tempPoint.collider.gameObject.tag == "SolarPanel" && !hasActivated)
        {
            hasActivated = true;
            OnActivated?.Invoke();
        }
    }


    }
