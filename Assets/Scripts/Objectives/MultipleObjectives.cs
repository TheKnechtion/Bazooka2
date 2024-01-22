using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleObjectives : Objective
{
    
    List<Objective> objectiveList;

    [SerializeField] GameObject[] objectives;

    [SerializeField] bool requireAllObjectivesToContinue;
    [SerializeField] int numberOfRequiredObjectives;

    int objectiveCount = 0;
    string objString;

    private void Awake()
    {
        objectiveList = new List<Objective>();
        
        foreach(GameObject objectiveObject in objectives)
        {
            objectiveObject.transform.TryGetComponent<Objective>(out Objective tempObj);
            objectiveList.Add(tempObj);
        }
        

        objectiveCount = objectiveList.Count();
        
        
        if(requireAllObjectivesToContinue)
        {
            objectiveCount = objectives.Count();
        }
        else
        {
            objectiveCount = numberOfRequiredObjectives;
        }
        

        ObjectiveCompleted = false;
        ObjectiveText = $"{objectiveCount} Objectives Left To Pass";
    }

    public override void IncrementTowardCompleteObjective()
    {
        currentCount++;

        ObjectiveText = $"{objectiveCount-currentCount} Objectives Left To Pass";

        if (currentCount == objectiveCount) 
        { 
            CompleteObjective();
        }
    }

    public override void DecrementAwayFromCompleteObjective()
    {
        currentCount--;

        ObjectiveText = $"{objectiveCount - currentCount} Objectives Left To Pass";

        if (currentCount != objectiveCount)
        {
            UncompleteObjective();
        }
    }




}
