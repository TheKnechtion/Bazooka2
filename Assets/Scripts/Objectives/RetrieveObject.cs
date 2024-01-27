using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveObject : Objective
{
    [SerializeField] Vector3 zoneCompletionSize;
    [SerializeField] Vector3 zoneOffset;

    [SerializeField] List<GameObject> objectiveGameObjects;
    [SerializeField] string objectiveName;

    [SerializeField] bool retrieveSpecificObject;
    [SerializeField] int numberOfObjectsToRetrieve;

    public LayerMask objectiveLayer;

    string objectiveTag;

    List<GameObject> overlappingGameObjects;
    List<GameObject> objectiveObjectsInZone;
    Collider[] hitColliders;

    bool didOnce = false;
    int hitCollidersCount = 0;
    bool hitColliderCountChanged = false;

    Vector3 zoneSize;

    private void Awake()
    {
        overlappingGameObjects = new List<GameObject>();
        objectiveObjectsInZone = new List<GameObject>();

        ObjectiveCompleted = false;

        if(!retrieveSpecificObject)
        {
            ObjectiveText = $"{numberOfObjectsToRetrieve - currentCount} {objectiveName} Required To Progress";
        }
        else
        {
            ObjectiveText = $"{objectiveName} Required To Progress";
        }


        if(!retrieveSpecificObject)
        {
            objectiveTag = objectiveGameObjects[0].tag;
        }

    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        CheckForColliderCountChange();

        if (retrieveSpecificObject && hitColliderCountChanged)
        {
            RetrieveObjects_CompletionCheck();
        }
        else if (hitColliderCountChanged)
        {
            RetrieveUnspecificObject_CompletionCheck();
        }

    }

    void CheckForColliderCountChange()
    {
        hitColliders = Physics.OverlapBox(transform.position + zoneOffset, zoneCompletionSize / 2, Quaternion.identity, objectiveLayer);

        if (hitCollidersCount != hitColliders.Length)
        {
            hitCollidersCount = hitColliders.Length;
            hitColliderCountChanged = true;
        }
        else
        {
            hitColliderCountChanged = false;
        }
    }

    void ClearLists()
    {
        objectiveObjectsInZone.Clear();
        overlappingGameObjects.Clear();
    }

    void AddCollidersToList()
    {
        int i = 0;

        while (i < hitColliders.Length)
        {
            overlappingGameObjects.Add(hitColliders[i].gameObject);
            i++;
        }
    }

    void CompletionCheck(bool conditionOne, bool conditionTwo)
    {
        if (conditionOne)
        {
            CompleteObjective();
        }
        else if (conditionTwo)
        {
            UncompleteObjective();
        }
    }


    void RetrieveUnspecificObject_CompletionCheck()
    {
        ClearLists();

        AddCollidersToList();

        currentCount = 0;

        foreach (GameObject overlappingGO in overlappingGameObjects)
        {
            if (objectiveTag == overlappingGO.tag)
            {
                currentCount++;
            }
        }
        ObjectiveText = $"{numberOfObjectsToRetrieve - currentCount} {objectiveName} Required To Progress";

        CompletionCheck(currentCount >= numberOfObjectsToRetrieve, ObjectiveCompleted);
    }
    
    void RetrieveObjects_CompletionCheck()
    {
        ClearLists();

        AddCollidersToList();

        foreach (GameObject overlappingGO in overlappingGameObjects)
        {
            if (objectiveGameObjects.Contains(overlappingGO))
            {
                objectiveObjectsInZone.Add(overlappingGO);
            }
        }

        CompletionCheck(objectiveObjectsInZone.Count == objectiveGameObjects.Count, ObjectiveCompleted);        
    }




    //DO NOT DELETE
    //This needs to be commented out when the game is built
    //This is a useful tool while developing to visually see the size of the zone the objective object needs to be places in.
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + zoneOffset, zoneCompletionSize);

    }


}
