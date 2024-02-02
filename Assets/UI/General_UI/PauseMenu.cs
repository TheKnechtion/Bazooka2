using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject resume;
    [SerializeField] GameObject quit;

    Button resumeButton;
    Button quitButton;

    Button currentButton;


    // Start is called before the first frame update
    void Start()
    {
        resumeButton = resume.GetComponent<Button>();
        quitButton = quit.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        //resumeButton.
        /*
        if(resumeButton.)
        {

        }
        */
    }



}
