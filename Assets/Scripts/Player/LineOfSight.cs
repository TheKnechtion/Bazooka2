using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask CheckingMask;
    private int CheckingMaskInt;

    [SerializeField] private LayerMask EnvironmentMask;
    [SerializeField] private float VisionRadius;

    [Tooltip("Angles in degrees that ISpottable will be spotted. Starts from Transform's forward.")]
    [SerializeField] private float SpottingAngle;
    private float RadSpotAngle;

    private Collider[] CollidersHit;
    int hitCount;

    private ISpottable spottableType;

    private Vector3 direction;
    private float distance;
    private float DotProduct;
    private float angleToTarget;

    private bool inVisionRange;

    // Start is called before the first frame update
    void Start()
    {
        CollidersHit = new Collider[4];
        hitCount = 0;

        RadSpotAngle = SpottingAngle*Mathf.Deg2Rad;

        direction = Vector3.zero;
        distance = 0.0f;

        CheckingMaskInt =  CheckingMask.value;
        int aksen = EnvironmentMask.value;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSpotting();
        //Debug.Log("Collider Count: "+hitCount);
    }

    private void CheckForSpotting()
    {
        hitCount = Physics.OverlapSphereNonAlloc(gameObject.transform.position, VisionRadius, CollidersHit, CheckingMaskInt);

        if (hitCount > 0) 
        {
            for (int i = 0; i < CollidersHit.Length; i++)
            {
                if (CollidersHit[i] != null &&
                    CollidersHit[i].gameObject.TryGetComponent<ISpottable>(out spottableType))
                {
                    direction = (CollidersHit[i].gameObject.transform.position - gameObject.transform.position).normalized;
                    distance = Vector3.Distance(gameObject.transform.position, CollidersHit[i].gameObject.transform.position);
                    DotProduct = Vector3.Dot(gameObject.transform.forward, direction);
                    angleToTarget = AngleToTarget(distance, transform.position, CollidersHit[i].transform.position);

                    if (DotProduct > RadSpotAngle && angleToTarget <= 30.0f )
                    {
                        if (!Physics.Raycast(gameObject.transform.position, direction, distance, EnvironmentMask))
                        {
                            spottableType.Spot();
                        }
                    }
                }
                
            }
        }
    }

    private float AngleToTarget(float hypDistance, Vector3 playerPos, Vector3 targetPos)
    {
        playerPos.y = 0;
        targetPos.y = 0;

        float xzDistance = Vector3.Distance(playerPos, targetPos);

        float angle = Mathf.Acos(xzDistance / hypDistance);


        angle *= Mathf.Rad2Deg;
        return angle;
    }
}
