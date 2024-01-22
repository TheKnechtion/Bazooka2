using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveZone : MonoBehaviour
{
    public bool ObjectiveCompleted;

    /*
    enum objectiveTypes
    {
        retrieveObject,
        endLevel,
        killEnemies,
        batteryHolder,
        solarPower,
        completeOtherObjective
    }
    */

    enum objectiveTypes
    {
        regularObjective,
        endLevel
    }


    //[SerializeField] objectiveTypes zoneObjective;

    [SerializeField] Material objectiveMaterial;

    [SerializeField] bool showObjectiveZone;
    [SerializeField] bool showObjectiveText;

    Light lightComponent;
    Material objMaterial;

    BatteryHolder batteryHolder_Objective;
    RetrieveObject retrieveObject_Objective;
    Objective zone_Objective;


    void Start()
    {
        

        if(showObjectiveZone)
        {
            lightComponent = this.gameObject.AddComponent<Light>();
            lightComponent.color = Color.red;


            objMaterial = new Material(objectiveMaterial);
            this.gameObject.GetComponent<MeshRenderer>().material = objMaterial;

        }
        else
        {
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
        }


        if(!this.gameObject.TryGetComponent<Objective>(out zone_Objective))
        {
            this.gameObject.transform.parent.TryGetComponent<Objective>(out zone_Objective);
        }

        
    }


    void Update()
    {
        CompletionCheck();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!ObjectiveCompleted)
        {
            ShowPlayerUI(other);
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (!ObjectiveCompleted)
        {
            ShowPlayerUI(other);
        }
        else
        {
            StopShowingPlayerUI(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopShowingPlayerUI(other);
    }

    void ShowPlayerUI(Collider other)
    {
        if (other.transform.tag == "Player" && showObjectiveText)
        {
            UI_Manager.Show_ObjectiveUI($"{zone_Objective.ObjectiveText}");
        }
    }


    void StopShowingPlayerUI(Collider other)
    {
        if (other.transform.tag == "Player")
        { 
            UI_Manager.StopShow_ObjectiveUI();
        }
    }

    void CompletionCheck()
    {
        if(zone_Objective.ObjectiveCompleted)
        {
            CompleteObjective();
        }
        else
        {
            UncompleteObjective();
        }
    }


    public void CompleteObjective()
    {
        ObjectiveCompleted = true;

        if(showObjectiveZone)
        {
            lightComponent.color = Color.green;
            objMaterial.SetColor("_Color", Color.green);
        }
    }

    public void UncompleteObjective()
    {
        ObjectiveCompleted = false;

        if (showObjectiveZone)
        {
            lightComponent.color = Color.red;
            objMaterial.SetColor("_Color", Color.red);
        }
    }
}
