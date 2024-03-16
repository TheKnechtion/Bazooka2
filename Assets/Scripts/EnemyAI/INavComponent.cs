using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavComponent 
{
    public abstract void MoveTo(Vector3 t);
    public abstract void StopMovement();
    public abstract void ResumeMovement();
    public abstract void CancelPath();
}
