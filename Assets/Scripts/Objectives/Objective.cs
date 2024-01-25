using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour, IObjective
{
    public UnityEvent onObjectiveComplete;
    public UnityEvent onObjectiveUncomplete;

    public bool ObjectiveCompleted { get; set; }

    public string ObjectiveText { get; set; }

    public int currentCount = 0;


    public void CompleteObjective()
    {
        ObjectiveCompleted = true;
        onObjectiveComplete?.Invoke();
    }

    public void UncompleteObjective()
    {
        ObjectiveCompleted = false;
        onObjectiveUncomplete?.Invoke();
    }

    public bool CheckCompletion()
    {
        return ObjectiveCompleted;
    }


    public virtual void IncrementTowardCompleteObjective()
    {
        currentCount++;
    }

    public virtual void DecrementAwayFromCompleteObjective()
    {
        currentCount--;
    }

}
