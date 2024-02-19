using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_UI : MonoBehaviour
{
    [SerializeField] private Material healthMaterial;
    [SerializeField] private float HealthAmount;

    [SerializeField]private GameObject HpBarObject;

    private void Awake()
    {
        if (healthMaterial == null && HpBarObject != null)
            healthMaterial = HpBarObject.GetComponent<MeshRenderer>().material;

        UpdateHealthbar(1.0f);
        ToggleHpBar(false);
    }
    public void UpdateHealthbar(float amount)
    {
        healthMaterial.SetFloat("_currentHealth", amount);
    }

    public void ToggleHpBar(bool toggle)
    {
        HpBarObject.SetActive(toggle);
    }

    public void ResetHealthBar()
    {
        UpdateHealthbar(1.0f);
        ToggleHpBar(false);
    }
}
