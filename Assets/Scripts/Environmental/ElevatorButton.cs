using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElevatorButton : MonoBehaviour
{
    GameObject playerCanvas;

    [SerializeField] GameObject Elevator;

    Elevator elevatorComp;

    Button buttonInstance;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            elevatorComp = Elevator.GetComponent<Elevator>();

            ElevatorUI.instance.Deactivate_ElevatorButtons();


            ElevatorUI.instance.Set_ElevatorButtons(elevatorComp);


            PlayerManager._playerController.PlayerActions.Shoot.Disable();

            return;
        }   

        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ElevatorUI.instance.Deactivate_ElevatorButtons();

            ElevatorUI.instance.StopShow_Elevator();


            PlayerManager._playerController.PlayerActions.Shoot.Enable();

            //UI_Manager.instance.

            return;
        }
    }



}
