using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableIndicator : MonoBehaviour
{
    private PlayerInfo refPlayerInfo;

    private SkinnedMeshRenderer render;
    private Material[] baseMaterials;
    private Material[] invulnerableMaterals;

    [SerializeField] private Material InvulnerableMat;
    private void Awake()
    {
        refPlayerInfo = GetComponentInParent<PlayerInfo>();

        render = GetComponent<SkinnedMeshRenderer>();
        if (render != null)
        {
            baseMaterials = render.materials;

            invulnerableMaterals = new Material[baseMaterials.Length+1];
            for (int i = 0; i < baseMaterials.Length; i++)
            {
                invulnerableMaterals[i] = baseMaterials[i];
            }

            invulnerableMaterals[invulnerableMaterals.Length - 1] = InvulnerableMat;
        }

        if (refPlayerInfo != null)
        {
            refPlayerInfo.OnVulnerableChange += OnChangeVulnerability;
        }
    }

    private void OnChangeVulnerability(object sender, bool e)
    {
        if (e)
        {
            render.materials = invulnerableMaterals;
        }
        else
        {
            render.materials = baseMaterials;
        }
    }

    private void OnDestroy()
    {
        refPlayerInfo.OnVulnerableChange -= OnChangeVulnerability;
    }
}
