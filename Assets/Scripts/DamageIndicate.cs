using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicate : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    //[SerializeField] Material defaultMaterial, damageMaterial;
    private Material bodyMaterial;
    private Renderer render;

    private EnemyBehavior enemy; //The specific enemy we are affecting;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();

        enemy= GetComponent<EnemyBehavior>();
        enemy.OnTakeDamage += EnemyBehavior_OnTakeDamage;
    
    }

    private void EnemyBehavior_OnTakeDamage(object sender, System.EventArgs e)
    {
        StartCoroutine(indicateDamage());
    }
    private IEnumerator indicateDamage()
    {
        Debug.Log("Changing materal");
        render.material = materials[1];

        yield return new WaitForSeconds(0.04f);

        Debug.Log("Default materal");
        render.material = materials[0];
    }
}
