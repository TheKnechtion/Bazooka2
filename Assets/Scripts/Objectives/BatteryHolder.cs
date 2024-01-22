using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatteryHolder : Objective
{
    //public UnityEvent OnBatteryPutInHolder;
    bool hasntActivated = true;

    private void Awake()
    {
        ObjectiveCompleted = false;
        ObjectiveText = $"Battery Required To Progress";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasntActivated && other.transform.tag == "Battery")
        {
            other.transform.SetParent(this.transform);

            other.transform.localPosition = new Vector3(0f,1.45f,-2.6f);
            other.transform.localRotation = Quaternion.Euler(0f,0f,0f);


            Destroy(other.transform.GetComponent<Rigidbody>());

            foreach (BoxCollider bc in other.transform.GetComponents<BoxCollider>())
            {
                bc.isTrigger = true;
            }

            if (other.transform.TryGetComponent<DragObject>(out DragObject dragObj))
            {
                dragObj.UseObject();
            }



            //OnBatteryPutInHolder.Invoke();
            hasntActivated = false;

            CompleteObjective();

            /*
            if (other.gameObject.GetComponent<Battery>().currentCharge > 0)
            {
                
                other.transform.SetParent(this.transform);
                
            };
            */

        }
    }




}
