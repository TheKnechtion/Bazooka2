using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] GameObject[] highResObjects;
    [SerializeField] GameObject[] lowResObjects;

    [SerializeField] bool doOnce;
    [SerializeField] bool loadHighRes;
    [SerializeField] bool loadLowRes;

    bool highResIsActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ActivateObjectResolutionSwap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && highResIsActive)
        {
            LoadLowRes();
        }
    }


    void ActivateObjectResolutionSwap()
    {
        if (loadHighRes && !highResIsActive)
        {
            LoadHighRes();
        }

        if (loadLowRes && !highResIsActive)
        {
            LoadLowRes();
        }

        if (doOnce)
        {
            Destroy(this.gameObject);
        }
    }



    void LoadHighRes()
    {
        highResIsActive = true;

        SetAllFalse(lowResObjects);

        SetAllActive(highResObjects);
    }


    void LoadLowRes()
    {
        highResIsActive = false;

        SetAllFalse(highResObjects);

        SetAllActive(lowResObjects);
    }


    void SetAllActive(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }

    void SetAllFalse(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }



}
