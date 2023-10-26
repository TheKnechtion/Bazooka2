using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyDatabase
{
    //holds all the stats data from the txt file
    private List<string> statTxtData = null;

    //used to import the enemy data from txt
    Importer importer = new Importer();

    //the indicator to begin recording the next stat in the incoming txt file data
    int increment = 0;

    //use to store the current enemy info being read in from the txt file
    EnemyInfo tempEnemyInfo;


    //string data holders to an empty value so they are ready for incoming data
    string nameData;
    string hpData;
    string mpData;
    string apData;
    string defData;


    //a list that will be used to store the actual enemy info
    public List<EnemyInfo> enemyDatabase { get; set; }

    
    //called by enemy spawn manager to create enemy database for in-game use
    public EnemyDatabase()
    {
        //generates a list that will be used to store the actual enemy info
        enemyDatabase = new List<EnemyInfo>();

        //imports the data
        ImportData();

    }

    void ImportData()
    {
        //imports the stat data for each enemy in the database
        statTxtData = importer.Import("Stats.txt");

        //read through the txt file, line by line
        foreach (string currentLine in statTxtData)
        {
            //set string data holders to an empty value so they are ready for incoming data
            nameData = "";
            hpData = "";
            mpData = "";
            apData = "";
            defData = "";


            //used to store the current enemy info being read in from the txt file
            tempEnemyInfo = new EnemyInfo();

            //set the indicator to begin recording the next stat in the data sequence equal to zero
            increment = 0;

            //iterate through each character in the current line
            for (int i = 0; i < currentLine.Length; i++)
            {
                //if the current character is a space, that is the indicator to begin recording the next stat
                if (currentLine[i] == ' ')
                {
                    increment++;
                }
                else
                {
                    //get name from txt file
                    nameData = (increment == 0) ? nameData += currentLine[i] : nameData;

                    //get HP from txt file
                    hpData = (increment == 1) ? hpData += currentLine[i] : hpData;

                    //get MP from txt file
                    mpData = (increment == 2) ? mpData += currentLine[i] : mpData;

                    //get AP from txt file
                    apData = (increment == 3) ? apData += currentLine[i] : apData;

                    //get DEF from txt file
                    defData = (increment == 4) ? defData + currentLine[i] : defData;
                }
            }

            //set name
            tempEnemyInfo.name = nameData;

            //if an of these parse throw an error, this will prevent it from stopping the program
            try
            {
                //set hp
                tempEnemyInfo.HP = int.Parse(hpData);

                //set mp
                tempEnemyInfo.MP = int.Parse(mpData);

                //set ap
                tempEnemyInfo.AP = int.Parse(apData);

                //set def
                tempEnemyInfo.DEF = int.Parse(defData);
            }
            catch
            {

            }

            //add the current enemy info to the enemy database list
            enemyDatabase.Add(tempEnemyInfo);

        }
    }


}
