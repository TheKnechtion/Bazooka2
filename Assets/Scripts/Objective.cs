using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{

    bool ObjectiveCompleted { get; set; }


    public void CompleteObjective();

    public void IncrementTowardCompleteObjective();


}
