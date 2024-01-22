using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveObject : Objective
{
    

    [SerializeField] Vector3 zoneCompletionSize;
    [SerializeField] Vector3 zoneOffset;
    [SerializeField] GameObject objectiveGameObject;
    [SerializeField] string objectiveName;
    public LayerMask objectiveLayer;

    List<GameObject> overlappingGameObjects;

    bool didOnce = false;
    int hitCollidersCount = 0;
    bool hitColliderCountChanged = false;

    Vector3 zoneSize;

    private void Awake()
    {
        overlappingGameObjects = new List<GameObject>();
        ObjectiveCompleted = false;
        ObjectiveText = $"{objectiveName} Required To Progress";
    }

    void FixedUpdate()
    {
        RetrieveObject_CompletionCheck();
    }

    void RetrieveObject_CompletionCheck()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position + zoneOffset, zoneCompletionSize/2, Quaternion.identity, objectiveLayer);

        if(hitCollidersCount != hitColliders.Length)
        {
            hitCollidersCount = hitColliders.Length;
            hitColliderCountChanged = true;
        }
        else
        {
            hitColliderCountChanged = false;
        }


        overlappingGameObjects.Clear();

        int i = 0;

        while (i < hitColliders.Length)
        {
            overlappingGameObjects.Add(hitColliders[i].gameObject);
            i++;
        }

        if (overlappingGameObjects.Contains(objectiveGameObject) && hitColliderCountChanged)
        {
            CompleteObjective();
        }
        else if(!overlappingGameObjects.Contains(objectiveGameObject) && hitColliderCountChanged)
        {
            UncompleteObjective();
        }
    }

    //DO NOT DELETE
    //This needs to be commented out when the game is built
    //This is a useful tool while developing to visually see the size of the zone the objective object needs to be places in.
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + zoneOffset, zoneCompletionSize);

    }


}
