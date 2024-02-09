using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial_Activatable : Activatable
{
    [SerializeField] Material materialOne;
    [SerializeField] Material materialTwo;

    Material currentMat;

    // Start is called before the first frame update
    void Start()
    {
        SetMaterial(materialOne);
    }


    public override void Activate()
    {
        isActive = true;
        SetMaterial(materialTwo);
    }


    public void SetMaterial(Material tempMaterial)
    {
        currentMat = new Material(tempMaterial);
        this.gameObject.GetComponent<MeshRenderer>().material = currentMat;
    }

}
