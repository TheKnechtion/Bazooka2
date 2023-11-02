using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicate : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    //[SerializeField] Material defaultMaterial, damageMaterial;
    private Material bodyMaterial;
    private Renderer render;

    [SerializeField] private Renderer[] renderers;

    private EnemyBehavior enemy; //The specific enemy we are affecting;
    private PlayerInfo player;

    // Start is called before the first frame update
    void Start()
    {
        if (renderers.Length == 0)
         render = GetComponent<Renderer>();

        if (gameObject.tag == "Player")
        {
            if (transform.parent.TryGetComponent<PlayerInfo>(out PlayerInfo info))
            {
                player = info;
                player.OnTakeDamage += OnDamaged;
            }
        }
        else
        {
            if (transform.parent.TryGetComponent<EnemyBehavior>(out EnemyBehavior en))
            {
                enemy = en;
                enemy.OnTakeDamage += OnDamaged;
            }
        }    
    
    }

    private void OnDamaged(object sender, System.EventArgs e)
    {
        if (renderers.Length > 0)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                StartCoroutine(indicateDamage(renderers[i]));
            }
        }
        else { StartCoroutine(indicateDamage(render)); }
        
    }

    //private void EnemyBehavior_OnTakeDamage(object sender, System.EventArgs e)
    //{
    //    StartCoroutine(indicateDamage());
    //}
    private IEnumerator indicateDamage(Renderer render)
    {
        //Debug.Log("Changing materal");
        render.material = materials[1];

        yield return new WaitForSeconds(0.5f);

        //Debug.Log("Default materal");
        render.material = materials[0];
    }
}
