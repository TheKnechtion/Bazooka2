using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound 
{
    //Name for lookup of clip
    public string Name;

    //The sound clip
    public AudioClip clip;

    //Volume for specific clip
    [Range(0,1)]
    public float Volume;

    //Loop setting for this sound
    public bool Loop;
}
