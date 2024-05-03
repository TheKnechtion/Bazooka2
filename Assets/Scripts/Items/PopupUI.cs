using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] Material number;
    [SerializeField] Material icon;
    [SerializeField] Texture2D[] numberTextures;


    Color startColor;

    Material tempMat;
    Material tempMatTwo;
    float currentAlpha;

    public int ammoToGain;
    Color textColor;



    private void Awake()
    {


        currentAlpha = 1f;

        tempMat = new Material(icon);
        tempMatTwo = new Material(number);

        tempMat.SetFloat("_alphaControl", currentAlpha);


        transform.GetChild(0).GetComponent<MeshRenderer>().material = tempMat;
        transform.GetChild(1).GetComponent<MeshRenderer>().material = tempMatTwo;
    }

    private void Start()
    {
        tempMatTwo.SetTexture("_MainTex", numberTextures[ammoToGain]);
    }

    private void FixedUpdate()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.01f, this.transform.localPosition.z + 0.01f);

        currentAlpha -= Time.deltaTime;

        tempMat.SetFloat("_alphaControl", Mathf.Clamp(currentAlpha, 0, 1));
        tempMatTwo.SetFloat("_alphaControl", Mathf.Clamp(currentAlpha, 0, 1));

        Destroy(this.gameObject, 3f);

    }

    public void Meh(int incomingValue)
    {
        ammoToGain = incomingValue;
    }



}
