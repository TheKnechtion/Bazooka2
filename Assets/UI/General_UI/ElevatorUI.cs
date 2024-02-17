using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEditor.Search;
using UnityEngine.UI;

public class ElevatorUI : MonoBehaviour
{
    [SerializeField] private GameObject ElevatorButtons;

    GameObject currentButton;
    Elevator currentElevator;
    UnityEngine.UI.Button currentButtonComponent;

    private static ElevatorUI _instance;

    public static ElevatorUI instance
    {
        get
        {
            return _instance;
        }
    }

    private void Update()
    {
        _instance = this;
    }


    public void Set_ElevatorButtons(Elevator refToElevator)
    {
        Cursor.visible = true;

        ElevatorButtons.SetActive(true);
        currentElevator = refToElevator;

        for (int i = 0; i < refToElevator.activeHeights.Count; i++)
        {
            currentButton = ElevatorButtons.transform.GetChild(i).gameObject;

            currentButtonComponent = currentButton.GetComponent<UnityEngine.UI.Button>();

            currentButtonComponent.interactable = refToElevator.activeHeights[i];

            currentButton.SetActive(true);
        }
    }

    public void Set_TargetFloor(int floor)
    {
        currentElevator.targetFloor = floor;
    }


    public void Deactivate_ElevatorButtons()
    {
        Cursor.visible = false;

        for (int i = 0; i < ElevatorButtons.transform.childCount; i++)
        {
            currentButton = ElevatorButtons.transform.GetChild(i).gameObject;

            currentButton.SetActive(false);
        }
    }


    public void Show_Elevator()
    {
        ElevatorButtons.SetActive(true);
    }
    public void StopShow_Elevator()
    {
        ElevatorButtons.SetActive(false);
    }
}
