using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatteryHolder : MonoBehaviour
{
    public UnityEvent OnBatteryPutInHolder;
    bool hasntActivated = true;


    
    private void OnTriggerEnter(Collider other)
    {
        if(hasntActivated && other.transform.tag == "Battery")
        {
            OnBatteryPutInHolder.Invoke();
            hasntActivated = false;

            /*
            if (other.gameObject.GetComponent<Battery>().currentCharge > 0)
            {
                
                other.transform.SetParent(this.transform);
                
            };
            */

        }
    }

}
