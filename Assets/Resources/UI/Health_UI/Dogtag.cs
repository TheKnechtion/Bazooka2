using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogtag : MonoBehaviour
{
    public Material DogTag;

    public Texture2D Turkey_One;
    public Texture2D Turkey_Two;
    public Texture2D Turkey_Three;
    public Texture2D Turkey_Four;
    public Texture2D Turkey_Five;
    public Texture2D Turkey_Six;

    int currentHP;
    int maximumHP;

    float filledHP = 1.00f;
    float emptyHP = 0.35f;


    string[] heartUnlocked = {"_Heart_One_Unlocked", "_Heart_Two_Unlocked", "_Heart_Three_Unlocked", "_Heart_Four_Unlocked", "_Heart_Five_Unlocked", "_Heart_Six_Unlocked", "_Heart_Seven_Unlocked", "_Heart_Eight_Unlocked", "_Heart_Nine_Unlocked", "_Heart_Ten_Unlocked", "_Heart_Eleven_Unlocked", "_Heart_Twelve_Unlocked", "_Heart_Thirteen_Unlocked", "_Heart_Fourteen_Unlocked", "_Heart_Fifteen_Unlocked"};
    string[] heartOpacity = { "_Heart_One_Opacity", "_Heart_Two_Opacity", "_Heart_Three_Opacity", "_Heart_Four_Opacity", "_Heart_Five_Opacity", "_Heart_Six_Opacity", "_Heart_Seven_Opacity", "_Heart_Eight_Opacity", "_Heart_Nine_Opacity", "_Heart_Ten_Opacity", "_Heart_Eleven_Opacity", "_Heart_Twelve_Opacity", "_Heart_Thirteen_Opacity", "_Heart_Fourteen_Opacity", "_Heart_Fifteen_Opacity" };



    // Start is called before the first frame update
    void Awake()
    {
        PlayerInfo.OnPlayerHpChange += SetHP;
        PlayerInfo.OnPlayerSpawn += SetHP;
    }


    int hp = 3;

    float divide;

    public void SetHP(object sender, System.EventArgs e)
    {
        currentHP = PlayerInfo.instance.currentHP;
        maximumHP = PlayerInfo.instance.maximumHP;

        divide = (float)currentHP / (float)maximumHP;

        //Set the number of unlocked hearts
        for (int i = 0; i < maximumHP; i++)
        {
            DogTag.SetInt(heartUnlocked[i], 1);
        }
        for (int i = heartUnlocked.Length-1; i > maximumHP-1; i--)
        {
            DogTag.SetInt(heartUnlocked[i], 0);
        }


        //Set the opacity of unlocked full hearts to be 1. 
        for (int i = 0; i < currentHP; i++)
        {
            DogTag.SetFloat(heartOpacity[i], filledHP);
        }

        //Set the opacity of unlocked empty hearts to be 0.35.
        for (int i = maximumHP - 1; i >= currentHP; i--)
        {
            DogTag.SetFloat(heartOpacity[i], emptyHP);
        }


        //Debug.Log(divide);

        if(divide == 0.0f || maximumHP == 0)
        {
            DogTag.SetTexture("_Turkey_Icon", Turkey_Six);
        }
        else if(divide <= 0.2f && divide > 0.0f)
        {
            DogTag.SetTexture("_Turkey_Icon",Turkey_Five);
        }
        else if(divide <= 0.4f && divide > 0.2f)
        {
            DogTag.SetTexture("_Turkey_Icon", Turkey_Four);
        }
        else if(divide <= 0.6f && divide > 0.4f)
        {
            DogTag.SetTexture("_Turkey_Icon", Turkey_Three);
        }
        else if (divide <= 0.8f && divide > 0.6f)
        {
            DogTag.SetTexture("_Turkey_Icon", Turkey_Two);
        }
        else if (divide <= 1.0f && divide > 0.8f)
        {
            DogTag.SetTexture("_Turkey_Icon", Turkey_One);
        }


    }



}
