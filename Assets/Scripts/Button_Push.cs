using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Button_Push : MonoBehaviour
{
  
    bool canActivate = false;

    Material activatedMat;
    MeshRenderer buttonRenderer;

    [SerializeField] private string displayObjectName;




    public UnityEvent OnActivated;



    private void Awake()                                    
    {
        PlayerManager.OnPlayerActivatePress += Activate;

        activatedMat = (Material)Resources.Load("Activated");
        buttonRenderer = gameObject.GetComponent<MeshRenderer>();

    }

    //activate the "press space" UI
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canActivate = true;
            UI_Manager.Show_InteractUI($"Activate {displayObjectName}");
        }
    }


    //de-activate the "press space" UI
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canActivate = false;
            UI_Manager.StopShow_InteractUI();
        }
    }


    public void Activate(object sender, System.EventArgs e)
    {
        if(canActivate)
        {
            buttonRenderer.material = activatedMat;

            OnActivated.Invoke();

            DeleteOnActivate();
        }
    }

    void DeleteOnActivate()
    {
        UI_Manager.StopShow_InteractUI();
        PlayerManager.OnPlayerActivatePress -= Activate;
        Destroy(this);
    }




}
