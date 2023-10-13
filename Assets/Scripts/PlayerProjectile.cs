using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public static event EventHandler OnExplosion;

    private void Awake()
    {
        PlayerManager.OnPlayerDetonate += PlayerManager_OnPlayerDetonate;
    }

    private void OnDestroy()
    {
        PlayerManager.activeProjectiles--;
        OnExplosion?.Invoke(this, EventArgs.Empty);
    }

    void PlayerManager_OnPlayerDetonate(object sender, System.EventArgs e)
    {
        PlayerManager.OnPlayerDetonate -= PlayerManager_OnPlayerDetonate;
        gameObject.GetComponent<Projectile>().DealSplashDamage();
        gameObject.GetComponent<Projectile>().DeleteProjectile();

    }



}
