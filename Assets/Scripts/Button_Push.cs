using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Button_Push : MonoBehaviour
{
  
    bool inRange = false;

    bool canActivate = true;

    Material inactiveMat;
    [SerializeField] Material activatedMat;
    MeshRenderer buttonRenderer;

    [SerializeField] private string displayObjectName;

    [SerializeField] bool buttonIsTurnedOff;


    [SerializeField] bool doOnce;



    public UnityEvent OnActivated;



    private void Awake()                                    
    {
        buttonRenderer = gameObject.GetComponent<MeshRenderer>();
        inactiveMat = buttonRenderer.material;
    }

    //activate the "press space" UI
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        if(buttonIsTurnedOff)
        {
            return;
        }


        PlayerManager.OnPlayerActivatePress += Activate;

        inRange = true;

        if(canActivate)
        {
            UI_Manager.Show_InteractUI($"Activate {displayObjectName}");
        }
    }


    //de-activate the "press space" UI
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        if (buttonIsTurnedOff)
        {
            return;
        }

        PlayerManager.OnPlayerActivatePress -= Activate;

        inRange = false;

        UI_Manager.StopShow_InteractUI();
        
    }


    public void TurnOnButton()
    {
        buttonIsTurnedOff = true;
    }

    public void TurnOffButton()
    {
        buttonIsTurnedOff = true;
    }

    public void Activate(object sender, System.EventArgs e)
    {
        if (inRange && canActivate)
        {
            buttonRenderer.material = activatedMat;

            UI_Manager.StopShow_InteractUI();

            GameObject.Find("GameManager").GetComponent<AudioManager>().PlayMiscClip("Button", transform.position);

            OnActivated.Invoke();

            canActivate = false;
        }

        if (doOnce)
        {
            DeleteOnActivate();
        }

        StartCoroutine(WaitToActivate());

    }

    void DeleteOnActivate()
    {
        UI_Manager.StopShow_InteractUI();
        PlayerManager.OnPlayerActivatePress -= Activate;
        Destroy(this);
    }
    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(3f);
        buttonRenderer.material = inactiveMat;

        if(inRange)
        {
            UI_Manager.Show_InteractUI($"Activate {displayObjectName}");
        }

        canActivate = true;
    }



}
