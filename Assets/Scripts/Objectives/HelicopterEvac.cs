using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEvac : Objective
{
    [SerializeField] string objectiveText;
    [SerializeField] bool useDefaultText;

    private void Awake()
    {
        if(useDefaultText)
        {
            ObjectiveText = objectiveText;
        }
        else
        {
            ObjectiveText = objectiveText;
        }

        ObjectiveCompleted = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(ObjectiveCompleted && other.transform.tag=="Player")
        {
            UI_Manager.Show_RoomSelect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ObjectiveCompleted && other.transform.tag == "Player")
        {
            UI_Manager.StopShow_RoomSelect();
        }
    }


}