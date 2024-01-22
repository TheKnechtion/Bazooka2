using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : Objective
{
    private void Awake()
    {
        ObjectiveCompleted = true;
        ObjectiveText = $"More Objectives Required To Progress";
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Player")
        {

        }

        if (ObjectiveCompleted && other.transform.tag == "Player")
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
