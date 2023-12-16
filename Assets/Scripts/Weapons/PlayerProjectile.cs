using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public static event EventHandler OnExplosion;

    private void Awake()
    {
        PlayerManager.activeProjectiles++;
    }

    private void OnDestroy()
    {
        PlayerManager.activeProjectiles--;
        OnExplosion?.Invoke(this, EventArgs.Empty);
    }





}
