using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;


public class MirrorLightReflection : Objective
{
    [SerializeField] LineRenderer lightBeams;

    //public UnityEvent OnActivated;
    //public UnityEvent OnDeactivated;


    private void Awake()
    {
        ObjectiveCompleted = false;
        ObjectiveText = $"Solar Power Required To Progress";
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
    string? objTag;
    bool tagChanged;

    bool activated = false;
    void BounceLightBeam()
    {
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
            tempPoint = mirrorPoint;
        }


        if (objTag != tempPoint.collider.gameObject.tag)
        {
            objTag = tempPoint.collider.gameObject.tag;
            tagChanged = true;
        }
        else
        {
            tagChanged = false;
        }


        if (tempPoint.collider.gameObject.tag == "SolarPanel" && tagChanged)
        {
            CompleteObjective();
        }
        else if(tempPoint.collider.gameObject.tag != "SolarPanel" && tagChanged && ObjectiveCompleted)
        {
            UncompleteObjective();
        }
    }


    }
