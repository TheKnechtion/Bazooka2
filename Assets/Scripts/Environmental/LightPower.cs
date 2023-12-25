using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPower : MonoBehaviour
{
    [SerializeField] GameObject flashlight_light;
    [SerializeField] GameObject flashlight_lightBeam;
    [SerializeField] GameObject flashlight;

    [SerializeField] Texture flashlight_on;
    [SerializeField] Texture flashlight_off;
    [SerializeField] Texture flashlight_emission;
    [SerializeField] Material flashlight_mat;

    bool isOn = true;

    float waitTime = 1f;

    Material tempMat;

    private void Awake()
    {
        tempMat = new Material(flashlight_mat);
        flashlight.GetComponent<MeshRenderer>().material = tempMat;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            AudioManager.PlayClipAtPosition("click",this.transform.position);

            if(isOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }


    void TurnOn()
    {
        flashlight_light.gameObject.SetActive(true);
        flashlight_lightBeam.gameObject.SetActive(true);
        tempMat.SetTexture("_BaseMap", flashlight_on);
        tempMat.EnableKeyword("_EMISSION");
        //flashlight_mat.SetTexture("_EmissionMap", flashlight_emission); 
        isOn = true;
    }

    void TurnOff()
    {
        flashlight_light.gameObject.SetActive(false);
        flashlight_lightBeam.gameObject.SetActive(false);

        tempMat.SetTexture("_BaseMap", flashlight_off);
        tempMat.DisableKeyword("_EMISSION");
        isOn = false;
    }

}
