using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ItemHover : MonoBehaviour
{
    private float yValue;
    public float incrementerVertical = 0;
    public float incrementerRotate = 0;
    private float maxIncrement = 3;

    [SerializeField] float yOffset;

    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Bounce();
        RotateY();
    }

    private void RotateY()
    {
        Quaternion xRotate = Quaternion.Euler(
                    0,
                    incrementerRotate,
                    0);

        gameObject.transform.rotation = xRotate;

        incrementerRotate += 1;
        if (incrementerRotate >= 360) incrementerRotate = 0;
    }

    private void Bounce()
    {
        yValue = 1.5f * Mathf.Abs(Mathf.Sin(incrementerVertical));

        gameObject.transform.position =
            new Vector3(
                gameObject.transform.position.x,
                yValue + yOffset,
                gameObject.transform.position.z);

        incrementerVertical += 0.05f;

        if (incrementerVertical >= maxIncrement) incrementerVertical = 0;
    }
}
