using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthForeground;
    float percentHealth;

    private Camera mainCamera; //Players virtual cam will always be main camera

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void ChangeStatus(int currentHP, int maxHP)
    {
        percentHealth = (float)currentHP/ (float)maxHP;
        healthForeground.fillAmount = percentHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
