using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEvac : Objective
{
    [SerializeField] string objectiveText;
    [SerializeField] bool useDefaultText;
    [SerializeField] private GameObject EvacZone;
    [SerializeField] private bool DisableZoneOnStart;
    [SerializeField] private bool CompleteByDefault;

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

        if(CompleteByDefault) { ObjectiveCompleted = true; }

        if (DisableZoneOnStart)
        {
            if (EvacZone != null)
            {
                EvacZone.SetActive(false);
            }
        }
    }

    private void Start()
    {
        //Should be for (Generic) Boss death event
        BehaviorTankBoss.OnTankKilled += Completed;
    }

    private void Completed(object sender, EventArgs e)
    {
        CompleteObjective();
        EvacZone.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectiveCompleted)
        {
            other.GetComponent<PlayerManager>().SetWeaponUsability(false);

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
        other.GetComponent<PlayerManager>().SetWeaponUsability(true);

        if (ObjectiveCompleted && other.transform.tag == "Player")
        {
            UI_Manager.StopShow_RoomSelect();
        }
    }

    private void Update()
    {
        Debug.Log("Obj completed: "+ObjectiveCompleted);
    }

}