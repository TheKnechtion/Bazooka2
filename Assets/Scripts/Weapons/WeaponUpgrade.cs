using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private UpgradeType type;

    public float FireRateAdd;
    public float WalkMuliplierAdd;
    public int MaxActiveProjectilesAdd;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Mask should only allow player collision

        if (collision != null)
        {
            //Look for the weapon that character currently has
            RangedWeapon temp = collision.gameObject.GetComponent<RangedWeapon>();

            switch (type)
            {
                case UpgradeType.FireRate:
                    temp.fireRate += FireRateAdd;
                    break;
                case UpgradeType.WalkMultiplier:
                    temp.walkMultiplier += WalkMuliplierAdd;
                    break;
                case UpgradeType.MaxActiveProjectile:
                    temp.maxActiveProjectiles += MaxActiveProjectilesAdd;
                    break;
                default:
                    break;
            }
        }
    }
}
