using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightControl : MonoBehaviour
{
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }


    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }

}
