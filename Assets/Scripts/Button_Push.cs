using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Button_Push : MonoBehaviour
{
  
    bool canActivate = false;

    Material inactiveMat;
    [SerializeField] Material activatedMat;
    MeshRenderer buttonRenderer;

    [SerializeField] private string displayObjectName;

    [SerializeField] bool doOnce;



    public UnityEvent OnActivated;



    private void Awake()                                    
    {
        PlayerManager.OnPlayerActivatePress += Activate;


        buttonRenderer = gameObject.GetComponent<MeshRenderer>();
        inactiveMat = buttonRenderer.material;
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
        if (canActivate)
        {
            buttonRenderer.material = activatedMat;

            OnActivated.Invoke();
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
        canActivate = true;
    }



}
