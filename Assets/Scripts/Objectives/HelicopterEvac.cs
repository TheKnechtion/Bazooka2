using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEvac : Objective
{
    [SerializeField] string objectiveText;
    [SerializeField] bool useDefaultText;

    public static event EventHandler MoveToNext;
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

    private void Start()
    {
        //Should be for (Generic) Boss death event
        BehaviorTankBoss.OnTankKilled += OnBossKilled;
    }

    private void OnBossKilled(object sender, System.EventArgs e)
    {
        ObjectiveCompleted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectiveCompleted)
        {
            if (LevelManager.GetHeldSceneCount() > 1)
            {
                UI_Manager.Show_RoomSelect();
            }
            else
            {
                MoveToNext.Invoke(this, EventArgs.Empty);
            }
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