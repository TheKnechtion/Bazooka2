using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class Exporter
{

    string path;

    //export's the results of a game
    public void Export(string result, int finalPlayerHP, string txtFileName)
    {
        //set the path for the txt file to the scripts file of the game project
        path = Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\";
        try
        {
            //create a file stream for the game results and create a txt file with a passed in name
            using (FileStream gameResults = new FileStream(path + txtFileName, FileMode.Create))
            {
                //create a stream writer for the txt file that was just created by the stream writer
                using (StreamWriter streamWriter = new StreamWriter(gameResults))
                {
                    //writes the whether the player is a winner or loser and their HP at the end of the game
                    streamWriter.WriteLine("You are a " + result);
                    streamWriter.WriteLine("Your final hp was " + finalPlayerHP);

                    //close the stream writer
                    streamWriter.Close();
                }

                //clsoe the game results
                gameResults.Close();
            }
        }
        catch
        {

        }
        



    }

    
}
