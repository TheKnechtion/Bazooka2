using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour
{
    public UnityEvent Listeners;

    private void Awake()
    {
        //Add possible events where needed as ELSE_IF
        if (gameObject.TryGetComponent<BehaviorTankBoss>(out BehaviorTankBoss btb))
        {
            btb.OnDeath += InvokedAction;
        }
    }

    private void InvokedAction(object sender, System.EventArgs e)
    {
        Listeners.Invoke();
    }
}
