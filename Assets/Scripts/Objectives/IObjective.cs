using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{

    public bool ObjectiveCompleted { get; set; }

    public string ObjectiveText { get; set; }

    public void CompleteObjective();

    public void IncrementTowardCompleteObjective();


}
