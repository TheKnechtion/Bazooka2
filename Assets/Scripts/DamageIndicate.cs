using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicate : MonoBehaviour
{
    //Materials of statuses
    [SerializeField] private Material[] SwapMaterials;
    
    //Base material array that will be reverted to
    private Material[] BaseMaterialArray;

    //This 'added' material signifies the entity takes more than 1 hit
    [SerializeField] private Material MoreHP_Material;

    //Possible sub-meshes
    private MeshFilter meshFilter;
    private MeshFilter[] SubMeshes;

    //The one renderer if no renderers are attatched
    private Renderer render;

    //Renderers set via inspector
    [SerializeField] private Renderer[] renderers;

    private EnemyBehavior enemy; //The specific enemy we are affecting;
    private PlayerInfo player; //The Player, if this component is attachted to player

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (renderers.Length == 0)
         render = GetComponent<Renderer>();

        if (render != null)
        {
            BaseMaterialArray = render.materials;
            //materials = render.material;
        }
        else if (renderers.Length > 0)
        {
            BaseMaterialArray = renderers[0].materials;
        }

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

                if (enemy.health > 1 && MoreHP_Material != null)
                {
                    Material[] MoreHp = new Material[BaseMaterialArray.Length + 1];
                    for (int i = 0; i < BaseMaterialArray.Length; i++)
                    {
                        MoreHp[i] = BaseMaterialArray[i];
                    }
                    MoreHp[MoreHp.Length - 1] = MoreHP_Material;

                    BaseMaterialArray = MoreHp;

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        renderers[i].materials = BaseMaterialArray;
                    }
                }
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
    private IEnumerator indicateDamage(Renderer render)
    {
        //Grabs 1st element in material array

        render.materials = SwapMaterials;

        yield return new WaitForSeconds(0.5f);

        if (enemy != null)
        {
            if (enemy.health < 2 && BaseMaterialArray.Length > 1)
            {
                Material[] lowHealth = new Material[BaseMaterialArray.Length - 1];
                lowHealth[0] = BaseMaterialArray[0];
                BaseMaterialArray = lowHealth;
            }
        }

        //Sets the material array to what it originally was
        render.materials = BaseMaterialArray;
    }
}
