using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtility:MonoBehaviour
{
    public static CoroutineUtility instance;

    public CoroutineUtility()
    {
        instance = this;
    }


    public static void CoroutineCheck(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            instance.StopCoroutine(coroutine);
        }
    }


}
