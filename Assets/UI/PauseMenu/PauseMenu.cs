using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public List<UnityEngine.UI.Button> buttons;

    public static event EventHandler<OnCycleSelectedMenuItem> OnSelectionChange;

    public class OnCycleSelectedMenuItem:EventArgs
    {
        public KeyCode keyPressed;
        public bool upPressed;
        public bool downPressed;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
