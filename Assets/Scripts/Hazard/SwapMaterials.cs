using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterials : Objective
{
    [SerializeField] Material materialOne;
    [SerializeField] Material materialTwo;

    Material currentMat;

    // Start is called before the first frame update
    void Start()
    {
        SetMaterial(materialOne);
    }

    public void Swap()
    {
        if(ObjectiveCompleted)
        {
            UncompleteObjective();
            SetMaterial(materialOne);
        }
        else
        {
            CompleteObjective();
            SetMaterial(materialTwo);
        }
    }


    public void SetMaterial(Material tempMaterial)
    {
        currentMat = new Material(tempMaterial);
        this.gameObject.GetComponent<MeshRenderer>().material = currentMat;
    }



}
