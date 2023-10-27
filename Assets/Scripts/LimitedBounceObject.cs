using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBounceObject : MonoBehaviour
{
    public int collisionsBeforeDestruction = 1;

    public void ProjectileCollision()
    {
        collisionsBeforeDestruction--;

        if (collisionsBeforeDestruction <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
