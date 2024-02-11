using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager 
{
    public static SceneScriptable NextScenes;
    public static void AddNextScenes(SceneScriptable next)
    {
        NextScenes = next;
    }
}
