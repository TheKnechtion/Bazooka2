using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    Quaternion targetQuat;
    float RotateY;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;


    [SerializeField] float sinPeriodIncrement;
    [SerializeField] float transformationAmplitude;

    float sinIncrement;
    [SerializeField] float centerHeight;


    [SerializeField] bool bladeMoves;
    [SerializeField] bool bladeRotates;


    float currentHeight = 0.0f;
    float RotateZ;

    private void Awake()
    {
        RotateY = transform.localEulerAngles.y;
        //RotateZ = transform.localEulerAngles.z;
    }

    private void FixedUpdate()
    {
        if(bladeMoves)
        {
            MoveBlade();
        }

        if(bladeRotates)
        {
            RotateBlade();
        }
    }

    void MoveBlade()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentHeight + centerHeight, gameObject.transform.position.z);

        currentHeight = transformationAmplitude * Mathf.Sin(sinIncrement);

        if (currentHeight > maxHeight) currentHeight = maxHeight;
        if (currentHeight < minHeight) currentHeight = minHeight;

        sinIncrement += sinPeriodIncrement;

        sinIncrement = (sinIncrement > 2 * Mathf.PI) ? 0.0f : sinIncrement;

    }
    void RotateBlade()
    {
        transform.rotation = Quaternion.Euler(-30f * Time.time, RotateY, 0);
    }

    public void StopRotating()
    {
        bladeRotates = false;
    }

    public void StopMoving()
    {
        bladeMoves = false;
    }

    public void StartRotating()
    {
        bladeRotates = true;
    }
    public void StartMoving()
    {
        bladeMoves = true;
    }



}
